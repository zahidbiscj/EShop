using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities;
using EShop.Core.Entities.Identity;
using EShop.Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace EShop.Data.Repositories
{
    public class PermissionRepository : BaseRepository<Permission,int>, IPermissionRepository
    {
        public PermissionRepository(EShopDbContext context) : base(context) { }

        public async Task<List<string>> GetPermissionsByUserId(int userId)
        {
            return await _context.RolePermissions
                .Include(x => x.Role)
                .Include(x => x.Permission)
                .Where(x => x.Role.UserRoles.Any(y => y.UserId == userId))
                .Select(x => x.Permission.PermissionName)
                .ToListAsync();
        }
    }
}
