using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProyectoRestaurante.Infrastructure.Data;

namespace ProyectoRestaurante.Infrastructure.Data.Configurations.DesignTime
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var cs = configuration.GetConnectionString("DefaultConnection")
                     ?? "Data Source=app.db";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(cs)
                .Options;

            return new AppDbContext(options);
        }
    }
}
