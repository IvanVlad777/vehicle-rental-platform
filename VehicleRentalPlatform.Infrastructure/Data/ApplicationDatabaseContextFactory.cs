using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Infrastructure.Data
{
    public class ApplicationDatabaseContextFactory: IDesignTimeDbContextFactory<ApplicationDatabaseContext>
    {
        public ApplicationDatabaseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDatabaseContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));

            return new ApplicationDatabaseContext(optionsBuilder.Options);
        }
    }
}
