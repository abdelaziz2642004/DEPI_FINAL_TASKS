using CinemaBooking.Data;
using CinemaBooking.Models;
using CinemaBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<AppUser> _users;

    public BookingController(AppDbContext db, UserManager<AppUser> users)
    {
        _db = db;
        _users = users;
    }

    [HttpGet]
    public async Task<IActionResult> Book(int id)
    {
        var showtime = await _db.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Hall).ThenInclude(h => h.Cinema)
            .Include(s => s.Bookings)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (showtime is null) return NotFound();
        if (showtime.HasStarted)
        {
            TempData["Error"] = "This showtime has already started.";
            return RedirectToAction("Details", "Movies", new { id = showtime.MovieId });
        }

        return View(new BookingViewModel { ShowtimeId = id, Showtime = showtime });
    }

    [HttpPost]
    public async Task<IActionResult> Book(BookingViewModel vm)
    {
        var showtime = await _db.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Hall).ThenInclude(h => h.Cinema)
            .Include(s => s.Bookings)
            .FirstOrDefaultAsync(s => s.Id == vm.ShowtimeId);

        if (showtime is null) return NotFound();

        vm.Showtime = showtime;

        if (!ModelState.IsValid) return View(vm);

        if (showtime.HasStarted)
        {
            TempData["Error"] = "Sorry, this showtime has already started.";
            return RedirectToAction("Details", "Movies", new { id = showtime.MovieId });
        }

        if (vm.NumberOfSeats > showtime.SeatsAvailable)
        {
            ModelState.AddModelError("NumberOfSeats", $"Only {showtime.SeatsAvailable} seat(s) left.");
            return View(vm);
        }

        var user = await _users.GetUserAsync(User);
        if (user is null) return Unauthorized();

        var booking = new Booking
        {
            UserId = user.Id,
            ShowtimeId = showtime.Id,
            NumberOfSeats = vm.NumberOfSeats,
            TotalPrice = vm.NumberOfSeats * showtime.TicketPrice
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        TempData["Success"] = $"Booking confirmed! {vm.NumberOfSeats} seat(s) for {showtime.Movie.Title}. Total: ${booking.TotalPrice:F2}";
        return RedirectToAction("MyBookings");
    }

    public async Task<IActionResult> MyBookings()
    {
        var user = await _users.GetUserAsync(User);
        if (user is null) return Unauthorized();

        var bookings = await _db.Bookings
            .Where(b => b.UserId == user.Id && !b.IsCancelled)
            .Include(b => b.Showtime).ThenInclude(s => s.Movie)
            .Include(b => b.Showtime).ThenInclude(s => s.Hall).ThenInclude(h => h.Cinema)
            .OrderByDescending(b => b.Showtime.StartTime)
            .ToListAsync();

        var vm = new MyBookingsViewModel
        {
            Upcoming = bookings.Where(b => !b.Showtime.HasStarted).ToList(),
            Past = bookings.Where(b => b.Showtime.HasStarted).ToList()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var user = await _users.GetUserAsync(User);
        var booking = await _db.Bookings
            .Include(b => b.Showtime)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == user!.Id);

        if (booking is null)
        {
            TempData["Error"] = "Booking not found.";
            return RedirectToAction("MyBookings");
        }

        if (booking.Showtime.HasStarted)
        {
            TempData["Error"] = "You cannot cancel a booking after the showtime has started.";
            return RedirectToAction("MyBookings");
        }

        booking.IsCancelled = true;
        await _db.SaveChangesAsync();

        TempData["Success"] = "Booking cancelled successfully.";
        return RedirectToAction("MyBookings");
    }
}
