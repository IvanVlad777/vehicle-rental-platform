using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Domain.Entities;
using VehicleRentalPlatform.Domain.Enums;
using VehicleRentalPlatform.Infrastructure.Data;

namespace VehicleRentalPlatform.Infrastructure.Repositories
{
    public class EFRentalRepository: IRentalRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;
        public EFRentalRepository(ApplicationDatabaseContext context)
        {
            _databaseContext = context;
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            return await _databaseContext.Rentals.AsNoTracking().ToListAsync();
        }

        public async Task<Rental?> GetByIdAsync(Guid id)
        {
            return await _databaseContext.Rentals.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Rental?> GetTrackedByIdAsync(Guid id)
        {
            return await _databaseContext.Rentals.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> HasOverlappingRentalAsync(Guid customerId, string vehicleVin, DateTime start, DateTime end)
        {
            return await _databaseContext.Rentals.AnyAsync(r =>
            r.CustomerId == customerId &&
            r.VehicleVin == vehicleVin &&
            r.Status == RentalStatus.Ordered &&
            r.StartTime < end &&
            r.EndTime > start);
        }

        public async Task<bool> HasOverlappingRentalExcludingAsync(Guid rentalId, Guid customerId, string vehicleVin, DateTime start, DateTime end)
        {
            return await _databaseContext.Rentals.AnyAsync(r =>
            r.Id != rentalId &&
            r.CustomerId == customerId &&
            r.VehicleVin == vehicleVin &&
            r.Status == RentalStatus.Ordered &&
            r.StartTime < end &&
            r.EndTime > start);
        }

        public async Task<IEnumerable<Rental>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _databaseContext.Rentals
                .Include(r => r.Vehicle)
                .Where(r => r.CustomerId == customerId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetByVehicleVinAsync(string vin)
        {
            return await _databaseContext.Rentals
                .Include(r => r.Vehicle)
                .Where(r => r.VehicleVin == vin)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Rental rental)
        {
            await _databaseContext.Rentals.AddAsync(rental);
        }

        public async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
