using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Application.Dtos.Rental
{
    public class RentalResponseDto
    {
        public Guid Id { get; set; }
        public string VehicleVin { get; set; } = null!;
        public Guid CustomerId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = null!;
        public int? StartOdometerKm { get; set; }
        public int? StartBatterySoc { get; set; }
        public int? EndBatterySoc { get; set; }
        public int? DistanceTraveled { get; set; }
        
    }
}
