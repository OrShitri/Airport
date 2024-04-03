using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportSimulator.Models
{
    public class FlightDto
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = null!;
        public string FlightStatus { get; set; } = null!;
        public int? CurrentLegId { get; set; }
        public string Brand { get; set; } = null!;
        public short? PaggengersCount { get; set; }
    }
}
