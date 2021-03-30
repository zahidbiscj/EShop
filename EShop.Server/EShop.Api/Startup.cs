using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EShop.Api.Configurations;
using EShop.Api.DependenciesRegister;
using EShop.Core.Constants;
using EShop.Core.Helpers;
using Serilog;
using EShop.Api.Extensions;
using EShop.Api.Seeders;
using EShop.Data;
using Microsoft.EntityFrameworkCore;

namespace EShop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Utils.RootPath = env.WebRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EShopDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString(ConfigurationConstants.ConnectionStringName));
            });

            services.AddAllRegisterDependencies(Configuration);
            services.SerilogConfigurationSetup(Configuration);

            services.AddJwtConfiguration(Configuration);
            services.AddCorsConfiguration();
            services.AddIdentityOptions();
            services.AddSwaggerConfiguration();

            services.AddAutoMapper(typeof(EShopDbContext));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseSeeder seeder)
        {
            seeder.EnsureDatabaseExists(app);
            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(ConfigurationConstants.SwaggerUrl, ConfigurationConstants.SwaggerName));
            }
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
