using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Application.Interfaces
{
    public interface ITelemetryRepository
    {
        Task<double?> GetLatestValueBeforeAsync(string vehicleVin, string name, DateTime before);
    }

}
