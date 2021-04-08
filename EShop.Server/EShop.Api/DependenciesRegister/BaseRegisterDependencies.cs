using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Api.Configurations;
using EShop.Api.Extensions;
using EShop.Api.Seeders;
using EShop.Core.Constants;
using EShop.Core.Helpers;
using EShop.Core.Interfaces.Others;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Api.DependenciesRegister
{
    public static class BaseRegisterDependencies
    {
        public static IServiceCollection AddAllRegisterDependencies(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddServicesDependency();
            services.AddRepositoriesDependency();

            services.AddScoped<IAuthorizationHandler, CustomAuthorizationExtension>();
            services.AddSingleton<ICurrentUser, CurrentUserService>();
            services.AddScoped<DatabaseSeeder>();
            services.Configure<SeedDataFilesConfiguration>(configuration.GetSection(AppConstants.SeedDataFileConfig));

            return services;
        }
    }
}
