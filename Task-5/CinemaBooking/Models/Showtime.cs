using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBooking.Models;

public class Showtime
{
    public int Id { get; set; }

    public DateTime StartTime { get; set; }

    [Column(TypeName = "decimal(8,2)")]
    public decimal TicketPrice { get; set; }

    public int MovieId { get; set; }
    public Movie Movie { get; set; } = null!;

    public int HallId { get; set; }
    public Hall Hall { get; set; } = null!;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public DateTime EndTime => StartTime.AddMinutes(Movie?.DurationMinutes ?? 0);

    public bool HasStarted => DateTime.UtcNow >= StartTime;

    public int SeatsBooked => Bookings?.Sum(b => b.NumberOfSeats) ?? 0;
    public int SeatsAvailable => (Hall?.SeatCapacity ?? 0) - SeatsBooked;
}
