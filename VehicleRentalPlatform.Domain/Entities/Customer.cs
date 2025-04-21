using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
