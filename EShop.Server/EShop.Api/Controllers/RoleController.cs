using Microsoft.AspNetCore.Http;
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
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Policy = AppPermissions.AddRoles)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RoleRequestModel model)
        {
            await _roleService.CreateRole(model);
            return Ok();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryModel model)
        {
            PagedResponse<RoleModel> data = await _roleService.GetRoles(model);
            return Ok(data);
        }
    }
}
