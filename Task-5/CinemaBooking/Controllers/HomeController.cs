using CinemaBooking.Data;
using CinemaBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var movies = await _db.Movies
            .AsNoTracking()
            .Include(m => m.Category)
            .Include(m => m.Showtimes)
            .OrderBy(m => m.Title)
            .Take(8)
            .ToListAsync();

        return View(movies);
    }

    [Route("/error/{code}")]
    public IActionResult Error(int code)
    {
        if (code == 404) return View("Error404");
        return View("Error500");
    }
}
