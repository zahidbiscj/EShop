using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Api.Configurations;
using EShop.Core.Constants;
using EShop.Core.Entities.Identity;
using EShop.Core.Helpers;
using EShop.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EShop.Api.Seeders
{
    public class DatabaseSeeder
    {
        private readonly SeedDataFilesConfiguration _fileConfig;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public DatabaseSeeder(IWebHostEnvironment hostEnvironment, IOptions<SeedDataFilesConfiguration> fileConfiguration,
            UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _hostEnvironment = hostEnvironment;
            _fileConfig = fileConfiguration.Value;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        public void Seed()
        {
            SeedRoles();
            SeedUsersWithRoles();
        }

        private void SeedUsersWithRoles()
        {
            var seedUsers = ReadJsonData<SeedUsersModel>(_fileConfig.UsersFileName);
            var users = _mapper.Map<List<User>>(seedUsers);
            foreach (var user in users)
            {
                var result = _userManager.CreateAsync(user, AppConstants.DefaultPassword).Result;
                if (result.Succeeded)
                {
                    var role = _roleManager.Roles.FirstOrDefault(x => x.Id == user.Id);
                    _userManager.AddToRoleAsync(user, role?.Name).Wait();
                }
            }
        }

        private void SeedRoles()
        {
            var seedRoles = ReadJsonData<SeedRolesModel>(_fileConfig.RolesFileName);
            var roles = _mapper.Map<List<Role>>(seedRoles);
            foreach (var role in roles)
            {
                _roleManager.CreateAsync(role).Wait();
            }
        }

        private List<T> ReadJsonData<T>(string fileName)
        {
            List<T> result = new List<T>();
            using (var file = File.OpenText(GetFullPath(fileName)))
            {
                result.AddRange(JsonConvert.DeserializeObject<List<T>>(file.ReadToEnd()));
            }
            return result;
        }

        private string GetFullPath(string fileName)
        {
            return Path.Combine(_hostEnvironment.WebRootPath, _fileConfig.RootFolder, fileName);
        }

        public void EnsureDatabaseExists(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<EShopDbContext>();

            context.Database.Migrate();
        }
    }
}
