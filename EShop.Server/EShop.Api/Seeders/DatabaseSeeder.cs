using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Api.Configurations;
using EShop.Core.Constants;
using EShop.Core.Entities.Identity;
using EShop.Core.Helpers;
using EShop.Core.Interfaces.IRepositories;
using EShop.Core.Interfaces.Others;
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
        private readonly IPermissionRepository _permissionRepository;
        private readonly EShopDbContext _context;

        public DatabaseSeeder(IWebHostEnvironment hostEnvironment, IOptions<SeedDataFilesConfiguration> fileConfiguration,
            UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IPermissionRepository permissionRepository, EShopDbContext context)
        {
            _hostEnvironment = hostEnvironment;
            _fileConfig = fileConfiguration.Value;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _permissionRepository = permissionRepository;
            _context = context;
        }

        public async Task Seed()
        {
            //await SeedPermissions();
            //await SeedRolesWithPermissions();
            //await SeedUsersWithRoles();
        }

        public async Task SeedPermissions()
        {
            var seedPermissions = ReadJsonData<Permission>(_fileConfig.PermissionsFilename);

            var all = await _permissionRepository.GetAllList();
            var newIds = seedPermissions.Select(x => x.Id)
                .Except(all.Select(x => x.Id)).ToList();

            await _permissionRepository.InsertRange(seedPermissions.Where(x => newIds.Contains(x.Id)));
            await WriteToDb(TableNames.Permissions);
            //seedPermissions.ForEach(seed =>
            //{
            //    var p = all.FirstOrDefault(x => x.Id == seed.Id);
            //    if (p != null)
            //    {
            //        p.Description = seed.Description;
            //        _permissionRepository.Update(p);
            //    }
            //});

        }

        private async Task WriteToDb(string tableName)
        {
            await _context.Database.OpenConnectionAsync();
            try
            {
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT dbo.{tableName} ON");
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT dbo.{tableName} OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        private async Task SeedUsersWithRoles()
        {
            var seedUsers = ReadJsonData<SeedUsersModel>(_fileConfig.UsersFileName);
            var users = _mapper.Map<List<User>>(seedUsers);
            foreach (var user in users)
            {
                var result = await _userManager.CreateAsync(user, AppConstants.DefaultPassword);
                if (result.Succeeded)
                {
                    var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == user.Id);
                    _userManager.AddToRoleAsync(user, role?.Name).Wait();
                }
            }
        }

        /*
         *  SeedRole -> Id,Name,Permissions{PermissionId}
         *  existingRole -> Id,Name,RolePermissions{roleId,PermissionId}
         *  RolePermissions -> RoleId, PermissionId
         * 1. SeedRoles er id er shathe role er id match korbo .. insert korbo jodi match na kore
         * 2. SeedRole er permission er shathe RolePermission er permission id match korbo 
         */
        private async Task SeedRolesWithPermissions()
        {
            var seedRoles = ReadJsonData<SeedRolesModel>(_fileConfig.RolesFileName);
            var existingRoles = await _roleManager.Roles.Include(x => x.RolePermissions).ToListAsync();

            foreach (var seedRole in seedRoles)
            {
                var existingRole = existingRoles.FirstOrDefault(x => x.Id == seedRole.Id);

                if (existingRole != null)
                {
                    var a = existingRole.RolePermissions.Select(x => x.PermissionId).AsQueryable();
                    var b = seedRole.Permissions.Select(permissionId => permissionId).AsQueryable();
                    var newPermissionId = b.Except(a).ToList();

                    foreach (var item in newPermissionId)
                    {
                        existingRole.RolePermissions.Add(new RolePermission()
                        {
                            PermissionId = item,
                            RoleId = existingRole.Id
                        });
                    }

                    await _roleManager.UpdateAsync(existingRole);
                }
                else
                {
                    var rolePermissions = new List<RolePermission>();
                    foreach (var seedRolePermissionId in seedRole.Permissions)
                    {
                        rolePermissions.Add(new RolePermission()
                        {
                            PermissionId = seedRolePermissionId,
                            RoleId = seedRole.Id
                        });
                    }
                    await _roleManager.CreateAsync(new Role()
                    {
                        Id = seedRole.Id,
                        Name = seedRole.Name,
                        RolePermissions = rolePermissions
                    });
                }
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
