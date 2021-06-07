using EShop.Core.Dto;
using EShop.Core.Dto.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Interfaces.IServices
{
    public interface ICategoryService
    {
        Task CreateCategory(CategoryRequestModel model);
        Task<PagedResponse<CategoryModel>> GetCategories(PaginationQueryModel model);
        Task<CategoryModel> GetCategory(int id);
        Task UpdateCategory(int id, CategoryRequestModel model);
        Task DeleteCategory(int id);
    }
}
