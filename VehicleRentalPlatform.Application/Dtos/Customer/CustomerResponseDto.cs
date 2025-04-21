using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Application.Dtos.Customer
{
    public class CustomerResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int TotalDistanceDriven { get; set; }
        public decimal TotalRentalPrice { get; set; }
    }
}
