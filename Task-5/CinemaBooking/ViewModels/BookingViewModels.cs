using CinemaBooking.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.ViewModels;

public class BookingViewModel
{
    public int ShowtimeId { get; set; }
    public Showtime? Showtime { get; set; }

    [Required]
    [Range(1, 20, ErrorMessage = "You can book between 1 and 20 seats.")]
    [Display(Name = "Number of Seats")]
    public int NumberOfSeats { get; set; } = 1;
}

public class MyBookingsViewModel
{
    public List<Booking> Upcoming { get; set; } = new();
    public List<Booking> Past { get; set; } = new();
}
