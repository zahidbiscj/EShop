using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Core.Dto.ResponseModels;
using EShop.Core.Entities.Identity;
using EShop.Core.Interfaces.IRepositories;
using EShop.Core.Interfaces.IServices;

namespace EShop.Service.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

        public async Task<List<string>> GetPermissionsByUserId(int userId)
        {
            return await _permissionRepository.GetPermissionsByUserId(userId);
        }

        public async Task<List<PermissionResponseModel>> GetAllPermissions()
        {
            var result = await _permissionRepository.GetAllList();
            return _mapper.Map<List<PermissionResponseModel>>(result);
        }
    }
}
