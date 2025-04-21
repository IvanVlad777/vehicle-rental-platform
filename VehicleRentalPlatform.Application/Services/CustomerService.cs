using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Dtos.Customer;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customers;
        private readonly IRentalRepository _rentals;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ICustomerRepository customers, IRentalRepository rentals, ILogger<CustomerService> logger)
        {
            _customers = customers;
            _rentals = rentals;
            _logger = logger;
        }

        public async Task<Customer> CreateAsync(CustomerCreateDto dto)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = dto.Name
            };

            await _customers.AddAsync(customer);
            await _customers.SaveChangesAsync();

            _logger.LogInformation("Created customer: {CustomerId}, Name: {Name}", customer.Id, customer.Name);
            return customer;
        }

        public async Task UpdateAsync(Guid id, CustomerUpdateDto dto)
        {
            var customer = await _customers.GetTrackedByIdAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Update failed. Customer not found: {CustomerId}", id);
                throw new Exception("Customer not found.");
            }

            customer.Name = dto.Name;
            await _customers.SaveChangesAsync();

            _logger.LogInformation("Updated customer: {CustomerId}, NewName: {Name}", customer.Id, customer.Name);
        }

        public async Task DeleteAsync(Guid id)
        {
            var customer = await _customers.GetTrackedByIdAsync(id);
            if (customer == null)
            {
                _logger.LogWarning("Delete failed. Customer not found: {CustomerId}", id);
                throw new Exception("Customer not found.");
            }

            _customers.Delete(customer);
            await _customers.SaveChangesAsync();

            _logger.LogInformation("Deleted customer: {CustomerId}", id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all customers.");
            return await _customers.GetAllAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Retrieving all customers.");
            return await _customers.GetByIdAsync(id);
        }

        public async Task<int> GetTotalDistanceAsync(Guid customerId)
        {
            _logger.LogInformation("Calculating total distance for customer: {CustomerId}", customerId);
            var rentals = await _rentals.GetByCustomerIdAsync(customerId);
            return rentals
                .Where(r => r.StartOdometerKm != null && r.EndOdometerKm != null)
                .Sum(r => r.EndOdometerKm!.Value - r.StartOdometerKm!.Value);
        }

        public async Task<decimal> GetTotalRentalPriceAsync(Guid customerId)
        {
            _logger.LogInformation("Calculating total rental price for customer: {CustomerId}", customerId);
            var rentals = await _rentals.GetByCustomerIdAsync(customerId);
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
