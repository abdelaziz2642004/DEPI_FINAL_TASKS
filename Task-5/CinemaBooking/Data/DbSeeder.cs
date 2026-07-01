using CinemaBooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<AppDbContext>();
        var userMgr = services.GetRequiredService<UserManager<AppUser>>();
        var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

        await db.Database.MigrateAsync();

        // Roles
        foreach (var role in new[] { "admin", "customer" })
        {
            if (!await roleMgr.RoleExistsAsync(role))
                await roleMgr.CreateAsync(new IdentityRole(role));
        }

        // Default admin account
        const string adminEmail = "admin@cinema.com";
        if (await userMgr.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "System Admin",
                EmailConfirmed = true
            };
            await userMgr.CreateAsync(admin, "Admin123!");
            await userMgr.AddToRoleAsync(admin, "admin");
        }

        // Sample data on first run
        if (await db.Categories.AnyAsync()) return;

        var categories = new[] { "Action", "Drama", "Comedy", "Horror", "Sci-Fi", "Romance" };
        var cats = categories.Select(n => new Category { Name = n }).ToList();
        db.Categories.AddRange(cats);
        await db.SaveChangesAsync();

        var movies = new[]
        {
            new Movie { Title = "Dune Part Two", Description = "Paul Atreides unites with the Fremen to seek revenge.", DurationMinutes = 166, Language = "English", ReleaseDate = new DateOnly(2024, 3, 1), CategoryId = cats[4].Id },
            new Movie { Title = "Oppenheimer", Description = "The story of J. Robert Oppenheimer's role in developing the atomic bomb.", DurationMinutes = 180, Language = "English", ReleaseDate = new DateOnly(2023, 7, 21), CategoryId = cats[1].Id },
            new Movie { Title = "The Fall Guy", Description = "A stuntman is pulled back for one last job.", DurationMinutes = 126, Language = "English", ReleaseDate = new DateOnly(2024, 5, 3), CategoryId = cats[0].Id },
            new Movie { Title = "Inside Out 2", Description = "Riley's mind welcomes new emotions.", DurationMinutes = 100, Language = "English", ReleaseDate = new DateOnly(2024, 6, 14), CategoryId = cats[2].Id },
        };
        db.Movies.AddRange(movies);
        await db.SaveChangesAsync();

        var cinema = new Cinema { Name = "Grand Cinemax", Location = "123 Main Street, Downtown", Description = "The city's premier cinema experience." };
        db.Cinemas.Add(cinema);
        await db.SaveChangesAsync();

        var halls = new[]
        {
            new Hall { Name = "Hall A", SeatCapacity = 120, CinemaId = cinema.Id },
            new Hall { Name = "Hall B", SeatCapacity = 80, CinemaId = cinema.Id },
            new Hall { Name = "IMAX Hall", SeatCapacity = 200, CinemaId = cinema.Id },
        };
        db.Halls.AddRange(halls);
        await db.SaveChangesAsync();

        var now = DateTime.UtcNow;
        var showtimes = new[]
        {
            new Showtime { MovieId = movies[0].Id, HallId = halls[2].Id, StartTime = now.AddDays(1).Date.AddHours(14), TicketPrice = 18.00m },
            new Showtime { MovieId = movies[0].Id, HallId = halls[2].Id, StartTime = now.AddDays(1).Date.AddHours(19), TicketPrice = 22.00m },
            new Showtime { MovieId = movies[1].Id, HallId = halls[0].Id, StartTime = now.AddDays(2).Date.AddHours(15), TicketPrice = 15.00m },
            new Showtime { MovieId = movies[2].Id, HallId = halls[1].Id, StartTime = now.AddDays(1).Date.AddHours(18), TicketPrice = 14.00m },
            new Showtime { MovieId = movies[3].Id, HallId = halls[0].Id, StartTime = now.AddDays(3).Date.AddHours(16), TicketPrice = 13.00m },
        };
        db.Showtimes.AddRange(showtimes);
        await db.SaveChangesAsync();
    }
}
