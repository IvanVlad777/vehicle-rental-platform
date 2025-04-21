using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Application.Interfaces
{
    public interface IRentalRepository
    {
        Task<bool> HasOverlappingRentalAsync(Guid customerId, string vehicleVin, DateTime start, DateTime end);
        Task<bool> HasOverlappingRentalExcludingAsync(Guid rentalId, Guid customerId, string vehicleVin, DateTime start, DateTime end); //note to self: isključuje trenutni rental
        Task AddAsync(Rental rental);
        Task SaveChangesAsync();
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(Guid id);
        Task<Rental?> GetTrackedByIdAsync(Guid id);
        Task<IEnumerable<Rental>> GetByCustomerIdAsync(Guid customerId);
        Task<IEnumerable<Rental>> GetByVehicleVinAsync(string vin);


    }
}
