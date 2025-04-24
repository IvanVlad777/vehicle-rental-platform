using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Infrastructure.Data.Seeders
{
    public class InitialDataSeeder
    {
        private readonly ApplicationDatabaseContext _context;
        private readonly ILogger<InitialDataSeeder> _logger;

        public InitialDataSeeder(ApplicationDatabaseContext context, ILogger<InitialDataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void SeedVehicles(string csvPath)
        {
            if (_context.Vehicles.Any())
            {
                _logger.LogInformation("Vehicles already seeded.");
                return;
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<VehicleCsvModel>().ToList();

            var vehicles = records.Select(r => new Vehicle
            {
                Vin = r.Vin,
                Make = r.Make,
                Model = r.Model,
                Year = r.Year,
                PricePerKmInEuro = r.PricePerKmInEuro,
                PricePerDayInEuro = r.PricePerDayInEuro
            });

            _context.Vehicles.AddRange(vehicles);
            _context.SaveChanges();

            _logger.LogInformation("Seeded {Count} vehicles from CSV.", records.Count);
        }

        public void SeedTelemetry(string csvPath)
        {
            if (_context.Telemetrys.Any())
            {
                _logger.LogInformation("Telemetry already seeded.");
                return;
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, config);

            var records = csv.GetRecords<TelemetryCsvModel>().ToList();

            var telemetryData = records.Where(r =>
            {
                var timestamp = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).UtcDateTime;
                var isValid = r.Value >= 0 && timestamp <= DateTime.UtcNow;
                if (!isValid)
                {
                    _logger.LogWarning("Skipping invalid telemetry: {Name}, value={Value}, timestamp={Timestamp}", r.Name, r.Value, timestamp);
                }
                return isValid;
            }).Select(r => new Telemetry
            {
                VehicleVin = r.Vin,
                Name = r.Name,
                Value = r.Value,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).UtcDateTime
            });

            _context.Telemetrys.AddRange(telemetryData);
            _context.SaveChanges();

            _logger.LogInformation("Seeded {Count} telemetry entries from CSV.", telemetryData.Count());
        }

        public void SeedUsers()
        {
            if(_context.Users.Any()) return;

            var hasher = new PasswordHasher<User>();

            var admin = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@vrp.com",
                Role = "Admin"
            };

            admin.PasswordHash = hasher.HashPassword(admin, "admin123");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@vrp.com",
                Role = "User"
            };

            user.PasswordHash = hasher.HashPassword(user, "user123");

            _context.Users.AddRange(admin, user);
            _context.SaveChanges();
        }

        private class VehicleCsvModel
        {
            [Name("vin")]
            public string Vin { get; set; } = null!;
            [Name("make")]
            public string Make { get; set; } = null!;
            [Name("model")]
            public string Model { get; set; } = null!;
            [Name("year")]
            public int Year { get; set; }
            [Name("pricePerKmInEuro")]
            public decimal PricePerKmInEuro { get; set; }
            [Name("pricePerDayInEuro")]
            public decimal PricePerDayInEuro { get; set; }
        }

        private class TelemetryCsvModel
        {
            [Name("vin")]
            public string Vin { get; set; } = null!;
            [Name("name")]
            public string Name { get; set; } = null!;
            [Name("value")]
            public double Value { get; set; }
            [Name("timestamp")]
            public long Timestamp { get; set; }
        }
    }
}
