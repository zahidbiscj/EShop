using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Entities.Identity;
using EShop.Core.Exceptions;
using EShop.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;

namespace EShop.Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;


        public RoleService(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task CreateRole(RoleRequestModel model)
        {
            var role = _mapper.Map<Role>(model);
            var result = await _roleManager.CreateAsync(role);
            
            if(!result.Succeeded)
                throw new DomainException(result.Errors.First().Description);
        }
    }
}
