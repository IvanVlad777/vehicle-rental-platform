using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Infrastructure.Data
{
    public class ApplicationDatabaseContext : DbContext
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options): base(options) { }

        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Rental> Rentals => Set<Rental>();
        public DbSet<Telemetry> Telemetrys => Set<Telemetry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
