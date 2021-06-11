using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EShop.Core.Dto;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Entities.Identity;
using EShop.Core.Exceptions;
using EShop.Core.Interfaces.IRepositories;
using EShop.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PagedResponse<RoleModel>> GetRoles(PaginationQueryModel model)
        {
            var query = _roleManager.Roles.AsQueryable();
            var data = query.ProjectTo<RoleModel>(_mapper.ConfigurationProvider).AsNoTracking();
            return await PagedResponse<RoleModel>.ApplyPaginationAsync(data, model.PageNo,model.PageSize);
        }
    }
}
