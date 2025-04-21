using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalPlatform.Domain.Entities
{
    public class Telemetry
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public double Value { get; set; }
        public DateTime Timestamp { get; set; }

        public string VehicleVin { get; set; } = null!;
        public Vehicle? Vehicle { get; set; }
    }
}
