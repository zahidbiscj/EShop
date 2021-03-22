using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Core.Constants;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Dto.ResponseModels;
using EShop.Core.Entities.Identity;
using EShop.Core.Exceptions;
using EShop.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager,
            IConfiguration configuration, IMapper mapper, IAuthService authService)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _authService = authService;
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
                    Token = await _authService.GenerateToken(user),
                    StatusCode = AppStatusCode.SuccessStatusCode.ToString(),
                    Message = string.Empty,
                    //UserProfile = await _userService.GetUserById(user.Id)
                };
            }

            throw new UnAuthorizedException(MessageConstants.UsernamePasswordDoNotMatch);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponseModel>> Register(RegisterRequestModel model)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == model.Email.ToLower()))
            {
                return BadRequest("Username is taken");
            }
            var user = _mapper.Map<User>(model);
            user.UserName = model.Email.ToLower();
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new RegisterResponseModel
            {
                //Username = user.UserName,
                //Token = await _tokenService.CreateToken(user)
            };
        }
    }
}
