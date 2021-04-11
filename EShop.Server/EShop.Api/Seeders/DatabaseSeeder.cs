using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Api.Configurations;
using EShop.Api.Helpers;
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
    public class DatabaseSeeder : BaseSeeder
    {
        private readonly SeedDataFilesConfiguration _fileConfig;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IPermissionRepository _permissionRepository;
        private readonly EShopDbContext _context;
        private readonly SeedIdentityHelper _seedIdentityHelper;

        public DatabaseSeeder(IWebHostEnvironment hostEnvironment, IOptions<SeedDataFilesConfiguration> fileConfiguration,
            UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IPermissionRepository permissionRepository, EShopDbContext context, SeedIdentityHelper seedIdentityHelper)
            : base(hostEnvironment, fileConfiguration, context)
        {
            _hostEnvironment = hostEnvironment;
            _fileConfig = fileConfiguration.Value;
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _permissionRepository = permissionRepository;
            _context = context;
            _seedIdentityHelper = seedIdentityHelper;
        }

        public async Task Seed()
        {
            await SeedPermissions();
            await SeedRolesWithPermissions();
            await SeedUsersWithRoles();
        }

        public async Task SeedPermissions()
        {
            var seedPermissions = ReadJsonData<Permission>(_fileConfig.PermissionsFilename);
            var existingPermissions = await _permissionRepository.GetAllList();

            var newIds = seedPermissions.Select(x => x.Id).Except(existingPermissions.Select(x => x.Id)).ToList();

            await _permissionRepository.InsertRange(seedPermissions.Where(x => newIds.Contains(x.Id)));

            DescriptionUpdateOfExistingPermission(seedPermissions, existingPermissions);

            await WriteToDb(TableNames.Permissions);
        }

        private void DescriptionUpdateOfExistingPermission(List<Permission> seedPermissions, IReadOnlyList<Permission> existingPermissions)
        {
            seedPermissions.ForEach(seedPermission =>
            {
                var permission = existingPermissions.FirstOrDefault(x => x.Id == seedPermission.Id);
                if (permission != null)
                {
                    permission.Description = seedPermission.Description;
                    _permissionRepository.Update(permission);
                }
            });
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

        public async Task SeedRolesWithPermissions()
        {
            var seedRoles = ReadJsonData<SeedRolesModel>(_fileConfig.RolesFileName);
            var existingRoles = await _roleManager.Roles.Include(x => x.RolePermissions).ToListAsync();

            foreach (var seedRole in seedRoles)
            {
                var existingRole = existingRoles.FirstOrDefault(x => x.Id == seedRole.Id);

                if (existingRole != null)
                {
                    await _seedIdentityHelper.UpdatePermissionsToExistingRole(seedRole, existingRole, this);
                }
                else
                {
                    await _seedIdentityHelper.InsertRoleWithPermissions(seedRole, this);
                }
            }
        }
    }
}
