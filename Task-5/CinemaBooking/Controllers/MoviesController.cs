using CinemaBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Controllers;

public class MoviesController : Controller
{
    private readonly AppDbContext _db;

    public MoviesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(int? categoryId, string? search)
    {
        var q = _db.Movies.AsNoTracking().Include(m => m.Category).AsQueryable();

        if (categoryId.HasValue) q = q.Where(m => m.CategoryId == categoryId.Value);
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(m => m.Title.Contains(search));

        ViewBag.Categories = await _db.Categories.AsNoTracking().ToListAsync();
        ViewBag.SelectedCategory = categoryId;
        ViewBag.Search = search;

        return View(await q.OrderBy(m => m.Title).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var movie = await _db.Movies
            .AsNoTracking()
            .Include(m => m.Category)
            .Include(m => m.Showtimes)
                .ThenInclude(s => s.Hall)
                    .ThenInclude(h => h.Cinema)
            .Include(m => m.Showtimes)
                .ThenInclude(s => s.Bookings)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie is null) return NotFound();

        var upcoming = movie.Showtimes
            .Where(s => s.StartTime > DateTime.UtcNow)
            .OrderBy(s => s.StartTime)
            .ToList();

        ViewBag.Showtimes = upcoming;
        return View(movie);
    }
}
