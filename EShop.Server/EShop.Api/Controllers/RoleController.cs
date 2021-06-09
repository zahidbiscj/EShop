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

        [Authorize(Policy = AppPermissions.ViewRoles)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQueryModel model)
        {
            PagedResponse<RoleModel> data = await _roleService.GetRoles(model);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewRoles)]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            RoleModel data = await _roleService.GetRoleById(id);
            return Ok(data);
        }

        [Authorize(Policy = AppPermissions.ViewRoles)]
        [HttpGet("Update")]
        public async Task<IActionResult> Update([FromQuery] RoleRequestModel model)
        {
            await _roleService.Update(model);
            return Ok();
        }

        [Authorize(Policy = AppPermissions.ViewRoles)]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _roleService.Delete(id);
            return Ok();
        }
    }
}
