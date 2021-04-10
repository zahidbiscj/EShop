using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Dto.ResponseModels;

namespace EShop.Core.Interfaces.IServices
{
    public interface IPermissionService
    {
        Task<List<PermissionResponseModel>> GetAllPermissions();
        Task<List<string>> GetPermissionsByUserId(int userId);
    }
}
