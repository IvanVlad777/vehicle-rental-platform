using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Dtos.Rental;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Application.Interfaces
{
    public interface IRentalService
    {
        Task<Rental> CreateRentalAsync(RentalCreateDto dto);
        Task<IEnumerable<Rental>> GetAllAsync();
        Task<Rental?> GetByIdAsync(Guid id);
        Task CancelAsync(Guid rentalId);
        Task UpdateAsync(Guid rentalId, DateTime newStart, DateTime newEnd);
    }
}
