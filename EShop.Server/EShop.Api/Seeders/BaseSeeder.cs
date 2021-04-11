using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EShop.Api.Configurations;
using EShop.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EShop.Api.Seeders
{
    public class BaseSeeder
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly SeedDataFilesConfiguration _fileConfig;
        private readonly EShopDbContext _context;


        public BaseSeeder(IWebHostEnvironment hostEnvironment, IOptions<SeedDataFilesConfiguration> fileConfiguration, EShopDbContext context)
        {
            _hostEnvironment = hostEnvironment;
            _fileConfig = fileConfiguration.Value;
            _context = context;
        }

        protected List<T> ReadJsonData<T>(string fileName)
        {
            List<T> result = new List<T>();
            using (var file = File.OpenText(GetFullPath(fileName)))
            {
                result.AddRange(JsonConvert.DeserializeObject<List<T>>(file.ReadToEnd()));
            }
            return result;
        }

        protected string GetFullPath(string fileName)
        {
            return Path.Combine(_hostEnvironment.WebRootPath, _fileConfig.RootFolder, fileName);
        }

        protected async Task WriteToDb(string tableName)
        {
            await _context.Database.OpenConnectionAsync();
            try
            {
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT dbo.{tableName} ON");
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT dbo.{tableName} OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        public void EnsureDatabaseExists(IApplicationBuilder applicationBuilder)
        {
            using var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<EShopDbContext>();

            context.Database.Migrate();
        }
    }
}
