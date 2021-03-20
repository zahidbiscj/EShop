using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.Core.Helpers;
using EShop.Core.Interfaces.Others;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Api.DependenciesRegister
{
    public static class BaseRegisterDependencies
    {
        public static IServiceCollection AddAllRegisterDependencies(this IServiceCollection services)
        {
            services.AddServicesDependency();
            services.AddRepositoriesDependency();

            services.AddSingleton<ICurrentUser, CurrentUserService>();

            return services;
        }
    }
}
