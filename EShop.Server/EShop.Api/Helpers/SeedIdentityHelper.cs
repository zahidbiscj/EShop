using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Core.Entities.Identity;
using EShop.Core.Helpers;
using EShop.Core.Interfaces.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace EShop.Api.Helpers
{
    public class SeedIdentityHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IPermissionRepository _permissionRepository;

        public SeedIdentityHelper(UserManager<User> userManager, RoleManager<Role> roleManager, IPermissionRepository permissionRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
        }
        public async Task InsertRoleWithPermissions(SeedRolesModel seedRole)
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

        public async Task UpdatePermissionsToExistingRole(SeedRolesModel seedRole, Role existingRole)
        {
            var newPermissionId = seedRole.Permissions.Where(seedPermissionId => existingRole.RolePermissions
                .All(y => y.PermissionId != seedPermissionId))
                .ToList();

            foreach (var permissionId in newPermissionId)
            {
                existingRole.RolePermissions.Add(new RolePermission()
                {
                    RoleId = existingRole.Id,
                    PermissionId = permissionId
                });
                await _roleManager.UpdateAsync(existingRole);
            }
        }

        public void DescriptionUpdateOfExistingPermission(List<Permission> seedPermissions, IReadOnlyList<Permission> existingPermissions)
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
    }
}
