using Microsoft.Extensions.Logging;
using VehicleRentalPlatform.Application.Dtos.Rental;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Domain.Entities;
using VehicleRentalPlatform.Domain.Enums;

namespace VehicleRentalPlatform.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentals;
        private readonly IVehicleRepository _vehicles;
        private readonly ICustomerRepository _customers;
        private readonly ITelemetryRepository _telemetry;

        private readonly ILogger<RentalService> _logger;

        public RentalService(IRentalRepository rentals, IVehicleRepository vehicles, ICustomerRepository customers, ITelemetryRepository telemetry, ILogger<RentalService> logger)
        {
            _vehicles = vehicles;
            _customers = customers;
            _rentals = rentals;
            _telemetry = telemetry;
            _logger = logger;
        }

        public async Task<Rental> CreateRentalAsync(RentalCreateDto dto)
        {
            _logger.LogInformation("Creating rental: Customer={CustomerId}, Vehicle={VehicleVin}, Start={Start}, End={End}",dto.CustomerId, dto.VehicleVin, dto.StartTime, dto.EndTime);

            var customer = await _customers.GetByIdAsync(dto.CustomerId) ?? throw new Exception("Customer not found.");

            var vehicle = await _vehicles.GetByVinAsync(dto.VehicleVin) ?? throw new Exception("Vehicle not found.");

            bool overlaping = await _rentals.HasOverlappingRentalAsync(dto.CustomerId, dto.VehicleVin, dto.StartTime, dto.EndTime);

            if (overlaping)
            {
                _logger.LogWarning("Rental rejected due to overlap: Customer={CustomerId}, Vehicle={VehicleVin}", dto.CustomerId, dto.VehicleVin);
                throw new Exception("Rental overlaps with existing reservation.");
            };

            var startOdometer = await _telemetry.GetLatestValueBeforeAsync(dto.VehicleVin, "odometer", dto.StartTime);
            var startBattery = await _telemetry.GetLatestValueBeforeAsync(dto.VehicleVin, "battery_soc", dto.StartTime);

            var rental = new Rental
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                VehicleVin = dto.VehicleVin,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = RentalStatus.Ordered,
                StartOdometerKm = startOdometer != null ? (int)startOdometer : null,
                StartBatterySoc = startBattery != null ? (int)startBattery : null
            };

            await _rentals.AddAsync(rental);
            await _rentals.SaveChangesAsync();

            _logger.LogInformation("Rental created successfully: {RentalId}", rental.Id);

            return rental;
        }

        public async Task<IEnumerable<Rental>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all rentals.");
            return await _rentals.GetAllAsync();
        }

        public async Task<Rental?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving rental by ID: {RentalId}", id);
            return await _rentals.GetByIdAsync(id);
        }

        public async Task CancelAsync(Guid id)
        {
            _logger.LogInformation("Cancelling rental: {RentalId}", id);

            var rental = await _rentals.GetTrackedByIdAsync(id) ?? throw new Exception("Rental not found.");

            if (rental.Status == RentalStatus.Cancelled)
            {
                _logger.LogWarning("Rental already cancelled: {RentalId}", id);
                throw new Exception("Rental is already cancelled.");
            }

            rental.Status = RentalStatus.Cancelled;

            await _rentals.SaveChangesAsync();

            _logger.LogInformation("Rental cancelled successfully: {RentalId}", id);
        }

        public async Task UpdateAsync(Guid id, DateTime newStart, DateTime newEnd)
        {
            _logger.LogInformation("Updating rental: {RentalId}, NewStart={Start}, NewEnd={End}", id, newStart, newEnd);

            var rental = await _rentals.GetTrackedByIdAsync(id) ?? throw new Exception("Rental not found.");
            if (rental.Status == RentalStatus.Cancelled)
            {
                _logger.LogWarning("Update failed. Rental is cancelled: {RentalId}", id);
                throw new Exception("Cannot update a cancelled rental.");
            }

            bool overlap = await _rentals.HasOverlappingRentalExcludingAsync(id, rental.CustomerId, rental.VehicleVin, newStart, newEnd);
            if (overlap)
            {
                _logger.LogWarning("Update rejected due to overlap: {RentalId}", id);
                throw new Exception("Updated rental overlaps with existing reservation.");
            }

            rental.StartTime = newStart;
            rental.EndTime = newEnd;

            await _rentals.SaveChangesAsync();

            _logger.LogInformation("Rental updated successfully: {RentalId}", id);
        }
    }
}
