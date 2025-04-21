using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Application.Dtos.Vehicle
{
    public class VehicleResponseDto
    {
        public string Vin { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public decimal PricePerKmInEuro { get; set; }
        public decimal PricePerDayInEuro { get; set; }

        public int TotalDistanceDriven { get; set; }
        public int TotalRentalCount { get; set; }
        public decimal TotalRentalIncome { get; set; }
    }
}
