using CinemaBooking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Areas.Admin.Controllers;

public class DashboardController : AdminBaseController
{
    private readonly AppDbContext _db;
    public DashboardController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalMovies = await _db.Movies.CountAsync();
        ViewBag.TotalCinemas = await _db.Cinemas.CountAsync();
        ViewBag.TotalBookings = await _db.Bookings.CountAsync(b => !b.IsCancelled);
        ViewBag.TotalRevenue = await _db.Bookings.Where(b => !b.IsCancelled).SumAsync(b => (decimal?)b.TotalPrice) ?? 0;
        return View();
    }
}
