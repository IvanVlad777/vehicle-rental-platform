using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Domain.Entities;
using VehicleRentalPlatform.Infrastructure.Data;

namespace VehicleRentalPlatform.Infrastructure.Repositories
{
    public class EFVehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;

        public EFVehicleRepository(ApplicationDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public async Task<Vehicle?> GetByVinAsync(string vin)
        {
            return await _databaseContext.Vehicles.FirstOrDefaultAsync(v => v.Vin == vin);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _databaseContext.Vehicles
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
