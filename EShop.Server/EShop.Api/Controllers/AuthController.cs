using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Constants;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Dto.ResponseModels;
using EShop.Core.Entities.Identity;
using EShop.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EShop.Api.Controllers
{
    public class AuthController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager,
            IConfiguration configuration)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<LoginResponseModel> Login(LoginRequestModel model)
        {
            //_logger.LogInformation("User {Email} is login", model.Email);
            var user = _userManager.Users.SingleOrDefault(x => x.Email == model.Email.Trim() && !x.IsDeleted);
            if (user == null)
            {
                throw new UnAuthorizedException(MessageConstants.UsernamePasswordDoNotMatch);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {
                return new LoginResponseModel()
                {
                    Token = await GenerateToken(user),
                    StatusCode = AppStatusCode.SuccessStatusCode.ToString(),
                    Message = string.Empty,
                    //UserProfile = await _userService.GetUserById(user.Id)
                };
            }

            throw new UnAuthorizedException(MessageConstants.UsernamePasswordDoNotMatch);
        }

        private async Task<string> GenerateToken(User user)
        {
            var userRoles = _roleManager.Roles
                .Where(x => x.UserRoles.Select(y => y.UserId).Contains(user.Id)).Select(x => x.Name).ToList();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, string.Join(',',userRoles))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
