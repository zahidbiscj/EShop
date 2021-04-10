using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities;
using EShop.Core.Entities.Identity;

namespace EShop.Core.Interfaces.IRepositories
{
    public interface IPermissionRepository : IBaseRepository<Permission, int>
    {
        Task<List<string>> GetPermissionsByUserId(int userId);
    }
}
