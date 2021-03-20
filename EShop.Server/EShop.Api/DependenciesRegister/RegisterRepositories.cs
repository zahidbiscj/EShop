using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Api.DependenciesRegister
{
    public static class RegisterRepositories
    {
        public static IServiceCollection AddRepositoriesDependency(this IServiceCollection services)
        {
            return services;
        }
    }
}
