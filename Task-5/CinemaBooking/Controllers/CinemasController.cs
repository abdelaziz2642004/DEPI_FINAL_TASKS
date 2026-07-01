using CinemaBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Controllers;

public class CinemasController : Controller
{
    private readonly AppDbContext _db;

    public CinemasController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var cinemas = await _db.Cinemas
            .AsNoTracking()
            .Include(c => c.Halls)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return View(cinemas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var cinema = await _db.Cinemas
            .AsNoTracking()
            .Include(c => c.Halls)
                .ThenInclude(h => h.Showtimes.Where(s => s.StartTime > DateTime.UtcNow))
                    .ThenInclude(s => s.Movie)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cinema is null) return NotFound();
        return View(cinema);
    }
}
