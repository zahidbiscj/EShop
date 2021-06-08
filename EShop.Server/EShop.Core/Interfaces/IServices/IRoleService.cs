using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Dto;
using EShop.Core.Dto.RequestModels;

namespace EShop.Core.Interfaces.IServices
{
    public interface IRoleService
    {
        Task CreateRole(RoleRequestModel model); 
        Task<PagedResponse<RoleModel>> GetRoles(PaginationQueryModel model);
        Task<RoleModel> GetRoleById(int id);
        Task Update(RoleRequestModel model);
        Task Delete(int id);
    }
}
