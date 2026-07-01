using BookStoreApi.Data;
using BookStoreApi.DTOs.Categories;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CategoryCreateDto dto);
    Task<(bool found, CategoryDto? result)> UpdateAsync(int id, CategoryCreateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;

    public CategoryService(AppDbContext db) => _db = db;

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        return await _db.Categories
            .AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                BookCount = c.Books.Count
            })
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        return await _db.Categories
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                BookCount = c.Books.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoryDto> CreateAsync(CategoryCreateDto dto)
    {
        var cat = new Category { Name = dto.Name };
        _db.Categories.Add(cat);
        await _db.SaveChangesAsync();
        return new CategoryDto { Id = cat.Id, Name = cat.Name, BookCount = 0 };
    }

    public async Task<(bool found, CategoryDto? result)> UpdateAsync(int id, CategoryCreateDto dto)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat is null) return (false, null);

        cat.Name = dto.Name;
        await _db.SaveChangesAsync();

        return (true, new CategoryDto { Id = cat.Id, Name = cat.Name });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cat = await _db.Categories.FindAsync(id);
        if (cat is null) return false;

        _db.Categories.Remove(cat);
        await _db.SaveChangesAsync();
        return true;
    }
}
