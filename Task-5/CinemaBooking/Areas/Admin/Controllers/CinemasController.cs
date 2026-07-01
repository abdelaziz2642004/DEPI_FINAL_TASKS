using CinemaBooking.Data;
using CinemaBooking.Models;
using CinemaBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Areas.Admin.Controllers;

public class CinemasController : AdminBaseController
{
    private readonly AppDbContext _db;
    public CinemasController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Cinemas.Include(c => c.Halls).OrderBy(c => c.Name).ToListAsync());

    [HttpGet]
    public IActionResult Create() => View(new CinemaFormViewModel());

    [HttpPost]
    public async Task<IActionResult> Create(CinemaFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        _db.Cinemas.Add(new Cinema { Name = vm.Name, Location = vm.Location, Description = vm.Description });
        await _db.SaveChangesAsync();
        TempData["Success"] = $"Cinema '{vm.Name}' created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var c = await _db.Cinemas.FindAsync(id);
        if (c is null) return NotFound();
        return View(new CinemaFormViewModel { Id = c.Id, Name = c.Name, Location = c.Location, Description = c.Description });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CinemaFormViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var c = await _db.Cinemas.FindAsync(vm.Id);
        if (c is null) return NotFound();
        c.Name = vm.Name; c.Location = vm.Location; c.Description = vm.Description;
        await _db.SaveChangesAsync();
        TempData["Success"] = "Cinema updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Cinemas.FindAsync(id);
        if (c is null) return NotFound();
        _db.Cinemas.Remove(c);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Cinema deleted.";
        return RedirectToAction(nameof(Index));
    }
}
