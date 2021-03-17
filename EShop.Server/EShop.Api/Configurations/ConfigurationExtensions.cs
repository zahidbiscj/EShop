using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EShop.Core.Constants;
using EShop.Core.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace EShop.Api.Configurations
{
    public static class ConfigurationExtensions
    {
        public static void SerilogConfigurationSetup(this IServiceCollection services, IConfiguration Configuration)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(Utils.RootPath, Configuration.GetSection(AppConstants.SerilogConfigFileName).Value))
                .Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }
    }
}
