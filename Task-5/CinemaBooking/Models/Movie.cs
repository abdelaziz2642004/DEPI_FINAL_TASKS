using System.ComponentModel.DataAnnotations;

namespace CinemaBooking.Models;

public class Movie
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(1, 600)]
    public int DurationMinutes { get; set; }

    [MaxLength(50)]
    public string Language { get; set; } = "English";

    public string? PosterPath { get; set; }

    public DateOnly ReleaseDate { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
