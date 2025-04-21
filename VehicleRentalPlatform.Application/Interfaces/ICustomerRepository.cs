using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Domain.Entities;

namespace VehicleRentalPlatform.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer?> GetTrackedByIdAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task AddAsync(Customer customer);
        void Delete(Customer customer);
        Task SaveChangesAsync();
    }
}
