using Microsoft.Extensions.Logging;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Application.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicles;
        private readonly IRentalRepository _rentals;
        private readonly ILogger<VehicleService> _logger;

        public VehicleService(IVehicleRepository vehicles, IRentalRepository rentals, ILogger<VehicleService> logger)
        {
            _vehicles = vehicles;
            _rentals = rentals;
            _logger = logger;
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all vehicles.");
            return await _vehicles.GetAllAsync();
        }

        public async Task<Vehicle?> GetByVinAsync(string vin)
        {
            _logger.LogInformation("Retrieving vehicle by VIN: {Vin}", vin);
            return await _vehicles.GetByVinAsync(vin);
        }

        public async Task<int> GetTotalDistanceAsync(string vin)
        {
            _logger.LogInformation("Calculating total distance for vehicle: {Vin}", vin);
            var rentals = await _rentals.GetByVehicleVinAsync(vin);
            return rentals
                .Where(r => r.StartOdometerKm != null && r.EndOdometerKm != null)
                .Sum(r => r.EndOdometerKm!.Value - r.StartOdometerKm!.Value);
        }

        public async Task<int> GetRentalCountAsync(string vin)
        {
            _logger.LogInformation("Calculating rental count for vehicle: {Vin}", vin);
            var rentals = await _rentals.GetByVehicleVinAsync(vin);
            return rentals.Count();
        }

        public async Task<decimal> GetTotalIncomeAsync(string vin)
        {
            _logger.LogInformation("Calculating total income for vehicle: {Vin}", vin);
            var rentals = await _rentals.GetByVehicleVinAsync(vin);
            return rentals
                .Where(r => r.Vehicle != null && r.StartOdometerKm != null && r.EndOdometerKm != null)
                .Sum(r =>
                    ((r.EndOdometerKm!.Value - r.StartOdometerKm!.Value) * r.Vehicle!.PricePerKmInEuro)
                    + (decimal)(r.EndTime - r.StartTime).TotalDays * r.Vehicle!.PricePerDayInEuro
                    + Math.Max(0, (r.StartBatterySoc ?? 0) - (r.EndBatterySoc ?? 0)) * 0.2m
                );
        }
    }
}
