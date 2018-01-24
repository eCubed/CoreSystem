using CoreSystem.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CoreSystem.DevDeploy2
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

            var connectionString = configuration["ConnectionString"];

            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("CoreSystem.DevDeploy2"));

            return new CoreSystemDbContext(builder.Options);
        }
    }
}
