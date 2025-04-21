using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Dtos.Rental;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Application.Services;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Tests.Unit
{
    public class RentalServiceTests
    {
        [Fact]
        public async Task CreateRentalAsync_ShouldThrow_WhenOverlapExists()
        {
            var rentals = new Mock<IRentalRepository>();
            var vehicles = new Mock<IVehicleRepository>();
            var customers = new Mock<ICustomerRepository>();
            var telemetry = new Mock<ITelemetryRepository>();
            var logger = new Mock<ILogger<RentalService>>();

            var customerId = Guid.NewGuid();
            var vin = "ABCDEFG1234567";
            var dto = new RentalCreateDto
            {
                CustomerId = customerId,
                VehicleVin = vin,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };

            rentals.Setup(r => r.HasOverlappingRentalAsync(customerId, vin, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            vehicles.Setup(v => v.GetByVinAsync(vin)).ReturnsAsync(new Vehicle());
            customers.Setup(c => c.GetByIdAsync(customerId)).ReturnsAsync(new Customer());

            var service = new RentalService(rentals.Object, vehicles.Object, customers.Object, telemetry.Object, logger.Object);

            Func<Task> action = async () => await service.CreateRentalAsync(dto);

            await action.Should().ThrowAsync<Exception>()
                .WithMessage("Rental overlaps with existing reservation.");
        }
    }
}
