using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Application.Dtos.Rental
{
    public class RentalUpdateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
