using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Interfaces.IRepositories;
using EShop.Core.Interfaces.IServices;

namespace EShop.Service.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<List<string>> GetPermissionsByUserId(int userId)
        {
            return await _permissionRepository.GetPermissionsByUserId(userId);
        }
    }
}
