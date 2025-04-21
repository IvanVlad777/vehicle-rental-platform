using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Dtos.Customer;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer> CreateAsync(CustomerCreateDto dto);
        Task UpdateAsync(Guid id, CustomerUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<decimal> GetTotalRentalPriceAsync(Guid customerId);
        Task<int> GetTotalDistanceAsync(Guid customerId);
    }
}
