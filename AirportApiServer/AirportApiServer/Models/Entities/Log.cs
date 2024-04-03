using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportApiServer.Models.Entities;

[Table("Log")]
public partial class Log
{
    [Key]
    public int Id { get; set; }

    public int FlightId { get; set; }

    public int LegId { get; set; }

    [StringLength(50)]
    public string FlightStatus { get; set; } = null!;

    public short PassengersCount { get; set; }

    [Column("In", TypeName = "datetime")]
    public DateTime InLeg { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Out { get; set; }

    [ForeignKey("FlightId")]
    [InverseProperty("Logs")]
    public virtual Flight Flight { get; set; } = null!;

    [ForeignKey("LegId")]
    [InverseProperty("Logs")]
    public virtual Leg Leg { get; set; } = null!;
}
