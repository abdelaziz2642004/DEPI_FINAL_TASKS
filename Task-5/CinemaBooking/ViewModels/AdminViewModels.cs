using CinemaBooking.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CinemaBooking.ViewModels;

public class MovieFormViewModel
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required, Range(1, 600)]
    [Display(Name = "Duration (minutes)")]
    public int DurationMinutes { get; set; }

    [Required, MaxLength(50)]
    public string Language { get; set; } = "English";

    [Required]
    [Display(Name = "Release Date")]
    public DateOnly ReleaseDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    public IFormFile? PosterFile { get; set; }
    public string? ExistingPoster { get; set; }

    public List<Category> Categories { get; set; } = new();
}

public class CinemaFormViewModel
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string Location { get; set; } = string.Empty;

    public string? Description { get; set; }
}

public class HallFormViewModel
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, Range(1, 1000)]
    [Display(Name = "Seat Capacity")]
    public int SeatCapacity { get; set; }

    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = string.Empty;
}

public class ShowtimeFormViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Movie")]
    public int MovieId { get; set; }

    [Required]
    [Display(Name = "Hall")]
    public int HallId { get; set; }

    [Required]
    [Display(Name = "Start Time")]
    public DateTime StartTime { get; set; } = DateTime.Now.AddDays(1);

    [Required, Range(1, 1000)]
    [Display(Name = "Ticket Price ($)")]
    public decimal TicketPrice { get; set; }

    public List<Movie> Movies { get; set; } = new();
    public List<Hall> Halls { get; set; } = new();
}
