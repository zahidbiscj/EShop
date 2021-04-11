using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Api.Seeders;
using EShop.Core.Entities.Identity;
using EShop.Core.Helpers;
using Microsoft.AspNetCore.Identity;

namespace EShop.Api.Helpers
{
    public class SeedIdentityHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public SeedIdentityHelper(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InsertRoleWithPermissions(SeedRolesModel seedRole, DatabaseSeeder databaseSeeder)
        {
            var rolePermissions = new List<RolePermission>();
            seedRole.Permissions.ForEach(seedRolePermissionId =>
            {
                rolePermissions.Add(new RolePermission()
                {
                    PermissionId = seedRolePermissionId,
                    RoleId = seedRole.Id
                });
            });

            await _roleManager.CreateAsync(new Role()
            {
                Id = seedRole.Id,
                Name = seedRole.Name,
                RolePermissions = rolePermissions
            });
        }

        public async Task UpdatePermissionsToExistingRole(SeedRolesModel seedRole, Role existingRole, DatabaseSeeder databaseSeeder)
        {
            var newPermissionId = seedRole.Permissions.Select(permissionId => permissionId).AsQueryable()
                .Except(existingRole.RolePermissions.Select(x => x.PermissionId).AsQueryable()).ToList();

            newPermissionId.ForEach(permissionId =>
                existingRole.RolePermissions.Add(new RolePermission()
                {
                    RoleId = existingRole.Id,
                    PermissionId = permissionId
                }));

            await _roleManager.UpdateAsync(existingRole);
        }
    }
}
