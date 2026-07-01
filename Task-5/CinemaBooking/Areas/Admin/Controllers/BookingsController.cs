using CinemaBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Areas.Admin.Controllers;

public class BookingsController : AdminBaseController
{
    private readonly AppDbContext _db;
    public BookingsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var bookings = await _db.Bookings
            .Include(b => b.User)
            .Include(b => b.Showtime).ThenInclude(s => s.Movie)
            .Include(b => b.Showtime).ThenInclude(s => s.Hall).ThenInclude(h => h.Cinema)
            .OrderByDescending(b => b.BookedAt)
            .ToListAsync();

        return View(bookings);
    }
}
