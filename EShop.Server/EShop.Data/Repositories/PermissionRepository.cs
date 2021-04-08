using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities.Identity;
using EShop.Core.Interfaces.IRepositories;

namespace EShop.Data.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        public Task<List<string>> GetPermissionsByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
