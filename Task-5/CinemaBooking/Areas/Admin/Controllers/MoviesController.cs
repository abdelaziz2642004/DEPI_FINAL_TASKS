using CinemaBooking.Data;
using CinemaBooking.Models;
using CinemaBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaBooking.Areas.Admin.Controllers;

public class MoviesController : AdminBaseController
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public MoviesController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var movies = await _db.Movies.Include(m => m.Category).OrderBy(m => m.Title).ToListAsync();
        return View(movies);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View(new MovieFormViewModel { Categories = await _db.Categories.ToListAsync() });
    }

    [HttpPost]
    public async Task<IActionResult> Create(MovieFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = await _db.Categories.ToListAsync();
            return View(vm);
        }

        var movie = new Movie
        {
            Title = vm.Title,
            Description = vm.Description,
            DurationMinutes = vm.DurationMinutes,
            Language = vm.Language,
            ReleaseDate = vm.ReleaseDate,
            CategoryId = vm.CategoryId,
            PosterPath = await SavePosterAsync(vm.PosterFile)
        };

        _db.Movies.Add(movie);
        await _db.SaveChangesAsync();
        TempData["Success"] = $"Movie '{movie.Title}' created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie is null) return NotFound();

        return View(new MovieFormViewModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            Language = movie.Language,
            ReleaseDate = movie.ReleaseDate,
            CategoryId = movie.CategoryId,
            ExistingPoster = movie.PosterPath,
            Categories = await _db.Categories.ToListAsync()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(MovieFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = await _db.Categories.ToListAsync();
            return View(vm);
        }

        var movie = await _db.Movies.FindAsync(vm.Id);
        if (movie is null) return NotFound();

        movie.Title = vm.Title;
        movie.Description = vm.Description;
        movie.DurationMinutes = vm.DurationMinutes;
        movie.Language = vm.Language;
        movie.ReleaseDate = vm.ReleaseDate;
        movie.CategoryId = vm.CategoryId;

        if (vm.PosterFile is not null)
            movie.PosterPath = await SavePosterAsync(vm.PosterFile);

        await _db.SaveChangesAsync();
        TempData["Success"] = $"Movie '{movie.Title}' updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie is null) return NotFound();

        _db.Movies.Remove(movie);
        await _db.SaveChangesAsync();
        TempData["Success"] = $"Movie deleted.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string?> SavePosterAsync(IFormFile? file)
    {
        if (file is null) return null;

        var folder = Path.Combine(_env.WebRootPath, "uploads", "posters");
        Directory.CreateDirectory(folder);
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(folder, fileName);
        await using var stream = System.IO.File.Create(path);
        await file.CopyToAsync(stream);
        return $"/uploads/posters/{fileName}";
    }
}
