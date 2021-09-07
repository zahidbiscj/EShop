using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EShop.Core.Constants;
using EShop.Core.Dto;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Entities.Core;
using EShop.Core.Exceptions;
using EShop.Core.Interfaces.IRepositories;
using EShop.Core.Interfaces.IServices;
using EShop.Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace EShop.Service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category,int> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IBaseRepository<Category, int> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task CreateCategory(CategoryRequestModel model)
        {
            
            var category = _mapper.Map<Category>(model);

            if (model.Id == 0) await _categoryRepository.Insert(category);
            else _categoryRepository.Update(category);

            await _categoryRepository.SaveAsync();

        }

        public async Task DeleteCategory(int id)
        {
            var categories =_categoryRepository.GetAll().Where(x => x.ParentCategoryId == id || x.Id==id).ToList();
            
            _categoryRepository.RemoveRange(categories);
            await _categoryRepository.SaveAsync();
        }

        public async Task<PagedResponse<CategoryModel>> GetCategories(PaginationQueryModel model)
        {
            var categories = _categoryRepository.GetAll();
            var allCategories = _mapper.Map<List<CategoryModel>>(categories);
            var query = allCategories.Where(x => x.ParentCategoryId == 1).Select(x => new CategoryModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ParentCategoryId = x.ParentCategoryId,
                SubCategories = GetSubCategoriesByParentId(allCategories, x.Id)
            }).AsQueryable();

            return await PagedResponse<CategoryModel>.ApplyPagination(query, model.PageNo, model.PageSize);
        }

        public async Task<CategoryModel> GetCategory(int id)
        {
            var entity = await _categoryRepository.GetById(id);
            var model = _mapper.Map<CategoryModel>(entity);
            return model;
        }

        public async Task UpdateCategory(int id, CategoryRequestModel model)
        {
            await CheckDuplicateName(model.Name);
            var oldCategory = await _categoryRepository.GetById(id);
            var updatedCategory = UpdateCategoryFromModel(oldCategory, model);
            _categoryRepository.Update(updatedCategory);
            await _categoryRepository.SaveAsync();
        }

        public async Task<PagedResponse<CategoryModel>> GetAllRootCategories(PaginationQueryModel model)
        {
            var query = _categoryRepository.GetAllWithSpecAsync(new BaseSpecification<Category>(c=>c.ParentCategoryId==null));
            var data = query.ProjectTo<CategoryModel>(_mapper.ConfigurationProvider).AsNoTracking();
            return await PagedResponse<CategoryModel>.ApplyPaginationAsync(data, model.PageNo, model.PageSize);
        }

        public async Task<List<CategoryModel>> GetSubCategories(int id)
        {
            var query = _categoryRepository.GetAllWithSpecAsync(new BaseSpecification<Category>(c => c.ParentCategoryId == id));
            var data = query.ProjectTo<CategoryModel>(_mapper.ConfigurationProvider).AsNoTracking();
            return await data.ToListAsync();
        }


        #region Supporting Method

        private static List<CategoryModel> GetSubCategoriesByParentId(List<CategoryModel> allCategories, int parentId)
        {
            var subCategories = allCategories.Where(x => x.ParentCategoryId == parentId)
                .Select(x => new CategoryModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ParentCategoryId = x.ParentCategoryId,
                    SubCategories = GetSubCategoriesByParentId(allCategories, x.Id)
                });

            return subCategories.ToList();
        } 

        private static Category UpdateCategoryFromModel(Category category, CategoryRequestModel model)
        {
            category.Name = model.Name;
            category.Description = model.Description;
            category.ParentCategoryId = model.ParentCategoryId;
            return category;
        }

        private async Task CheckDuplicateName(string categoryName)
        {
            var duplicateCategory =
                await _categoryRepository.GetEntityWithSpec(new BaseSpecification<Category>(x => x.Name == categoryName));
            if (duplicateCategory != null)
                throw new ConflictException(MessageConstants.CategoryAlreadyExist);
        }

        #endregion
    }
}
