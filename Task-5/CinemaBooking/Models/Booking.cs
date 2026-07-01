using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBooking.Models;

public class Booking
{
    public int Id { get; set; }

    public int NumberOfSeats { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    public DateTime BookedAt { get; set; } = DateTime.UtcNow;

    public bool IsCancelled { get; set; }

    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; } = null!;

    public int ShowtimeId { get; set; }
    public Showtime Showtime { get; set; } = null!;
}
