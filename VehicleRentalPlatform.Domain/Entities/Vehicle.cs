using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public string Vin { get; set; } = null!;
        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;
        public int Year { get; set; }

        public decimal PricePerKmInEuro { get; set; }
        public decimal PricePerDayInEuro { get; set; }

        public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    }
}
