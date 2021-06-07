using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Core.Constants;
using EShop.Core.Dto;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;

namespace EShop.Api.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Policy = AppPermissions.AddCategories)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CategoryRequestModel model)
        {
            await _categoryService.CreateCategory(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ViewCategories)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryModel model)
        {
            var data = await _categoryService.GetCategories(model);
            return Ok(data);
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _categoryService.GetCategory(id);
            return Ok(data);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryRequestModel model)
        {
            await _categoryService.UpdateCategory(id, model);
            return Ok();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}
