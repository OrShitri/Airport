using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportApiServer.Models.Entities;

[Table("NextLeg")]
public partial class NextLeg
{
    [Key]
    public int Id { get; set; }

    public int Leg { get; set; }

    public int Next { get; set; }
}
