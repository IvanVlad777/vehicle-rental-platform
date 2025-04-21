using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Application.Interfaces
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByVinAsync(string vin);
    }
}
