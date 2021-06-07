﻿using System;
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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task CreateCategory(CategoryRequestModel model)
        {
            await CheckDuplicateName(model.Name);
            var category = _mapper.Map<Category>(model);
            await _categoryRepository.Insert(category);
            await _categoryRepository.SaveAsync();
        }

        public async Task DeleteCategory(int id)
        {
            await _categoryRepository.RemoveAsync(id);
            await _categoryRepository.SaveAsync();
        }

        public async Task<PagedResponse<CategoryModel>> GetCategories(PaginationQueryModel model)
        {
            var query = _categoryRepository.GetAll();
            var data = query.ProjectTo<CategoryModel>(_mapper.ConfigurationProvider).AsNoTracking();
            return await PagedResponse<CategoryModel>.ApplyPagination(data, model.PageNo, model.PageSize);
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
        

        #region Supporting Method

        private static Category UpdateCategoryFromModel(Category category, CategoryRequestModel model)
        {
            category.Name = model.Name;
            category.Description = model.Description;
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
