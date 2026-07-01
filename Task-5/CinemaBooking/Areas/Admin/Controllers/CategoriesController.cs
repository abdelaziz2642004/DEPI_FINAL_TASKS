using CinemaBooking.Data;
using CinemaBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Areas.Admin.Controllers;

public class CategoriesController : AdminBaseController
{
    private readonly AppDbContext _db;
    public CategoriesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Categories.Include(c => c.Movies).OrderBy(c => c.Name).ToListAsync());

    [HttpGet]
    public IActionResult Create() => View(new Category());

    [HttpPost]
    public async Task<IActionResult> Create(Category model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.Categories.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = $"Category '{model.Name}' created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat is null) return NotFound();
        return View(cat);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Category model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.Categories.Update(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Category updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat is null) return NotFound();
        _db.Categories.Remove(cat);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Category deleted.";
        return RedirectToAction(nameof(Index));
    }
}
