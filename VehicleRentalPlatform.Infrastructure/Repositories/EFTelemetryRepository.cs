using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Infrastructure.Data;

namespace VehicleRentalPlatform.Infrastructure.Repositories
{
    public class EFTelemetryRepository : ITelemetryRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;

        public EFTelemetryRepository(ApplicationDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<double?> GetLatestValueBeforeAsync(string vehicleVin, string name, DateTime before)
        {
            return await _databaseContext.Telemetrys
                .Where(t => t.VehicleVin == vehicleVin && t.Name == name && t.Timestamp <= before)
                .OrderByDescending(t => t.Timestamp)
                .Select(t => (double?)t.Value)
                .FirstOrDefaultAsync();
        }
    }
}
