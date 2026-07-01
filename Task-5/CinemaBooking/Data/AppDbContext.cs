using CinemaBooking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Cinema> Cinemas => Set<Cinema>();
    public DbSet<Hall> Halls => Set<Hall>();
    public DbSet<Showtime> Showtimes => Set<Showtime>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Movie>(e =>
        {
            e.HasOne(m => m.Category)
             .WithMany(c => c.Movies)
             .HasForeignKey(m => m.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Hall>(e =>
        {
            e.HasOne(h => h.Cinema)
             .WithMany(c => c.Halls)
             .HasForeignKey(h => h.CinemaId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Showtime>(e =>
        {
            e.HasOne(s => s.Movie)
             .WithMany(m => m.Showtimes)
             .HasForeignKey(s => s.MovieId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(s => s.Hall)
             .WithMany(h => h.Showtimes)
             .HasForeignKey(s => s.HallId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<Booking>(e =>
        {
            e.HasOne(b => b.User)
             .WithMany(u => u.Bookings)
             .HasForeignKey(b => b.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(b => b.Showtime)
             .WithMany(s => s.Bookings)
             .HasForeignKey(b => b.ShowtimeId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
