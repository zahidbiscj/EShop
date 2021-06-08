using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EShop.Core.Dto;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Entities;
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
            IdentityResult result = await _roleManager.RoleExistsAsync(role.Name) ?
                                                await _roleManager.UpdateAsync(role) :
                                                await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new DomainException(result.Errors.First().Description);
        }

        public async Task<PagedResponse<RoleModel>> GetRoles(PaginationQueryModel model)
        {
            var data = _roleManager.Roles.AsQueryable()
                                            .ProjectTo<RoleModel>(_mapper.ConfigurationProvider)
                                            .AsNoTracking();

            return await PagedResponse<RoleModel>.ApplyPagination(data, model.PageNo,model.PageSize);
        }

        public async Task<RoleModel> GetRoleById(int id)
        {
            return await _roleManager.Roles.Where(x => x.Id == id).AsQueryable()
                                    .ProjectTo<RoleModel>(_mapper.ConfigurationProvider).AsNoTracking()
                                    .FirstOrDefaultAsync();
        }

        public Task Update(RoleRequestModel model)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            var existingRole = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            await _roleManager.DeleteAsync(existingRole);
        }
    }
}
