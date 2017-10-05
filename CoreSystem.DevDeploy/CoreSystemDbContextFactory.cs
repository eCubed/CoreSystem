using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using CoreSystem.EntityFramework;

namespace CoreSystem.DevDeploy
{
    public class CoreSystemDbContextFactory : IDesignTimeDbContextFactory<CoreSystemDbContext>
    {
        public CoreSystemDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var builder = new DbContextOptionsBuilder<CoreSystemDbContext>();

            var connectionString = configuration["CoreSystemConnectionString"];

            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("CoreSystem.DevDeploy"));

            return new CoreSystemDbContext(builder.Options);
        }
    }
}
