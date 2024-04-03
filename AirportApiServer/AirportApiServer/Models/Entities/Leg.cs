using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportApiServer.Models.Entities;

[Table("Leg")]
public partial class Leg
{
    [Key]
    public int Id { get; set; }

    public int LegNumber { get; set; }

    public int CrossingTime { get; set; }

    [InverseProperty("CurrentLeg")]
    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    [InverseProperty("Leg")]
    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
