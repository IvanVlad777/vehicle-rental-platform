using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRentalPlatform.Domain.Enums;

namespace VehicleRentalPlatform.Domain.Entities
{
    public class Rental
    {
        public Guid Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public RentalStatus Status { get; set; }

        public int? StartOdometerKm { get; set; }
        public int? EndOdometerKm { get; set; }

        public int? StartBatterySoc { get; set; }
        public int? EndBatterySoc { get; set; }

        

        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public string VehicleVin { get; set; } = null!;
        public Vehicle? Vehicle { get; set; }
    }
}
