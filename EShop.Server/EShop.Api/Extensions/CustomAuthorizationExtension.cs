using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Core.Constants;
using EShop.Core.Interfaces.IServices;
using EShop.Core.Interfaces.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace EShop.Api.Extensions
{
    public class CustomAuthorizationExtension : AuthorizationHandler<ClaimsAuthorizationRequirement>
    { 
        private readonly ICurrentUser _currentUser;
        private readonly IPermissionService _permissionService;

        public CustomAuthorizationExtension(ICurrentUser currentUser, IPermissionService permissionService)
        {
            _currentUser = currentUser;
            _permissionService = permissionService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ClaimsAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }

            var claims = await _permissionService.GetPermissionsByUserId(_currentUser.UserId);
            this._currentUser.SetClaims(claims.Select(claim => new System.Security.Claims.Claim(AppConstants.Permission, claim)));

            foreach (var item in requirement.AllowedValues)
            {
                if (!claims.Contains(item))
                {
                    context.Fail();
                    return;
                }
            }
            context.Succeed(requirement);
        }
    }
}
