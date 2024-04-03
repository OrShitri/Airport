using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportApiServer.Models.Entities;

[Table("Flight")]
public partial class Flight
{
    [Key]
    public int Id { get; set; }

    public string FlightNumber { get; set; } = null!;

    [StringLength(50)]
    public string FlightStatus { get; set; } = null!;

    public int? CurrentLegId { get; set; }

    [StringLength(50)]
    public string Brand { get; set; } = null!;

    public short? PaggengersCount { get; set; }

    [ForeignKey("CurrentLegId")]
    [InverseProperty("Flights")]
    public virtual Leg? CurrentLeg
    {
        get => currentLeg; set
        {
            currentLeg = value!;

            if (currentLeg != null)
            {
                LegTimer.Change(0, currentLeg.CrossingTime);
                StatusChangedEvent?.Invoke(this);
            }
        }
    }

    [InverseProperty("Flight")]
    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public Timer LegTimer;

    public bool GetOut = false;

    private Leg currentLeg;
    public Flight()
    {
        LegTimer = new Timer(MyCallback);
    }

    private void MyCallback(object? state)
    {
        CallMyCallback?.Invoke(this);
    }

    public event Action<Flight> CallMyCallback;
    public event Action<Flight> StatusChangedEvent;
}
