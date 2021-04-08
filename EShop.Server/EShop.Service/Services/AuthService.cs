using EShop.Core.Entities.Identity;
using EShop.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EShop.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService( UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> GenerateToken(User user)
        {
            var userRoles = await _roleManager.Roles
                .Where(x => x.UserRoles.Select(y => y.UserId).Contains(user.Id))
                .Select(x => x.Name).ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Role, string.Join(',',userRoles))
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
