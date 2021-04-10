using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Core.Interfaces.IServices;
using EShop.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Api.DependenciesRegister
{
    public static class RegisterServices
    {
        public static IServiceCollection AddServicesDependency(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPermissionService,PermissionService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
