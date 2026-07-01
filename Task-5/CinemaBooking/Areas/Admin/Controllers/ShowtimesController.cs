using CinemaBooking.Data;
using CinemaBooking.Models;
using CinemaBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Areas.Admin.Controllers;

public class ShowtimesController : AdminBaseController
{
    private readonly AppDbContext _db;
    public ShowtimesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var showtimes = await _db.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Hall).ThenInclude(h => h.Cinema)
            .Include(s => s.Bookings)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
        return View(showtimes);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View(new ShowtimeFormViewModel
        {
            Movies = await _db.Movies.OrderBy(m => m.Title).ToListAsync(),
            Halls = await _db.Halls.Include(h => h.Cinema).OrderBy(h => h.Cinema.Name).ThenBy(h => h.Name).ToListAsync()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(ShowtimeFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Movies = await _db.Movies.OrderBy(m => m.Title).ToListAsync();
            vm.Halls = await _db.Halls.Include(h => h.Cinema).ToListAsync();
            return View(vm);
        }

        _db.Showtimes.Add(new Showtime
        {
            MovieId = vm.MovieId,
            HallId = vm.HallId,
            StartTime = vm.StartTime.ToUniversalTime(),
            TicketPrice = vm.TicketPrice
        });
        await _db.SaveChangesAsync();
        TempData["Success"] = "Showtime created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var s = await _db.Showtimes.FindAsync(id);
        if (s is null) return NotFound();
        return View(new ShowtimeFormViewModel
        {
            Id = s.Id, MovieId = s.MovieId, HallId = s.HallId,
            StartTime = s.StartTime, TicketPrice = s.TicketPrice,
            Movies = await _db.Movies.OrderBy(m => m.Title).ToListAsync(),
            Halls = await _db.Halls.Include(h => h.Cinema).ToListAsync()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ShowtimeFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Movies = await _db.Movies.OrderBy(m => m.Title).ToListAsync();
            vm.Halls = await _db.Halls.Include(h => h.Cinema).ToListAsync();
            return View(vm);
        }
        var s = await _db.Showtimes.FindAsync(vm.Id);
        if (s is null) return NotFound();
        s.MovieId = vm.MovieId; s.HallId = vm.HallId;
        s.StartTime = vm.StartTime.ToUniversalTime(); s.TicketPrice = vm.TicketPrice;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Showtime updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var s = await _db.Showtimes.FindAsync(id);
        if (s is null) return NotFound();
        _db.Showtimes.Remove(s);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Showtime deleted.";
        return RedirectToAction(nameof(Index));
    }
}
