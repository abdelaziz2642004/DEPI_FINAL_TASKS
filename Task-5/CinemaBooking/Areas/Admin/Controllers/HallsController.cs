using CinemaBooking.Data;
using CinemaBooking.Models;
using CinemaBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Areas.Admin.Controllers;

public class HallsController : AdminBaseController
{
    private readonly AppDbContext _db;
    public HallsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(int cinemaId)
    {
        var cinema = await _db.Cinemas.Include(c => c.Halls).FirstOrDefaultAsync(c => c.Id == cinemaId);
        if (cinema is null) return NotFound();
        ViewBag.Cinema = cinema;
        return View(cinema.Halls.ToList());
    }

    [HttpGet]
    public async Task<IActionResult> Create(int cinemaId)
    {
        var cinema = await _db.Cinemas.FindAsync(cinemaId);
        if (cinema is null) return NotFound();
        return View(new HallFormViewModel { CinemaId = cinemaId, CinemaName = cinema.Name });
    }

    [HttpPost]
    public async Task<IActionResult> Create(HallFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        _db.Halls.Add(new Hall { Name = vm.Name, SeatCapacity = vm.SeatCapacity, CinemaId = vm.CinemaId });
        await _db.SaveChangesAsync();
        TempData["Success"] = $"Hall '{vm.Name}' created.";
        return RedirectToAction(nameof(Index), new { cinemaId = vm.CinemaId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var hall = await _db.Halls.Include(h => h.Cinema).FirstOrDefaultAsync(h => h.Id == id);
        if (hall is null) return NotFound();
        return View(new HallFormViewModel { Id = hall.Id, Name = hall.Name, SeatCapacity = hall.SeatCapacity, CinemaId = hall.CinemaId, CinemaName = hall.Cinema.Name });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(HallFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var hall = await _db.Halls.FindAsync(vm.Id);
        if (hall is null) return NotFound();
        hall.Name = vm.Name; hall.SeatCapacity = vm.SeatCapacity;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Hall updated.";
        return RedirectToAction(nameof(Index), new { cinemaId = hall.CinemaId });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var hall = await _db.Halls.FindAsync(id);
        if (hall is null) return NotFound();
        var cinemaId = hall.CinemaId;
        _db.Halls.Remove(hall);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Hall deleted.";
        return RedirectToAction(nameof(Index), new { cinemaId });
    }
}
