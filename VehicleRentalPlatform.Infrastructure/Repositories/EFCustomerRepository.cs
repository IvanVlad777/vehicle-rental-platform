using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Application.Interfaces;
using VehicleRentalPlatform.Domain.Entities;
using VehicleRentalPlatform.Infrastructure.Data;

namespace VehicleRentalPlatform.Infrastructure.Repositories
{
    public class EFCustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDatabaseContext _databaseContext;

        public EFCustomerRepository(ApplicationDatabaseContext context)
        {
            _databaseContext = context;
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _databaseContext.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetTrackedByIdAsync(Guid id)
        {
            return await _databaseContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _databaseContext.Customers.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Customer customer)
        {
            await _databaseContext.Customers.AddAsync(customer);
        }

        public void Delete(Customer customer)
        {
            _databaseContext.Customers.Remove(customer);
        }

        public async Task SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
