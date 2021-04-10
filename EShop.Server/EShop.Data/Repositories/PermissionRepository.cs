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
    public class PermissionRepository : IPermissionRepository
    {
        private readonly EShopDbContext _context;
        private Hashtable _repositories;

        public PermissionRepository(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<List<string>> GetPermissionsByUserId(int userId)
        {
            return await _context.RolePermissions
                .Include(x => x.Role)
                .Include(x => x.Permission)
                .Where(x => x.Role.UserRoles.Any(y => y.UserId == userId))
                .Select(x => x.Permission.PermissionName)
                .ToListAsync();
        }

        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepository<TEntity>)_repositories[type];
        }
    }
}
