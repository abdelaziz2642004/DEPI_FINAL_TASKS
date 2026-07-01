using BookStoreApi.Data;
using BookStoreApi.DTOs.Authors;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Services;

public interface IAuthorService
{
    Task<List<AuthorDto>> GetAllAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<AuthorDto> CreateAsync(AuthorCreateDto dto);
    Task<(bool found, AuthorDto? result)> UpdateAsync(int id, AuthorCreateDto dto);
    Task<bool> DeleteAsync(int id);
}

public class AuthorService : IAuthorService
{
    private readonly AppDbContext _db;

    public AuthorService(AppDbContext db) => _db = db;

    public async Task<List<AuthorDto>> GetAllAsync()
    {
        return await _db.Authors
            .AsNoTracking()
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BookCount = a.Books.Count
            })
            .OrderBy(a => a.LastName)
            .ToListAsync();
    }

    public async Task<AuthorDto?> GetByIdAsync(int id)
    {
        return await _db.Authors
            .AsNoTracking()
            .Where(a => a.Id == id)
            .Select(a => new AuthorDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                BookCount = a.Books.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<AuthorDto> CreateAsync(AuthorCreateDto dto)
    {
        var author = new Author { FirstName = dto.FirstName, LastName = dto.LastName };
        _db.Authors.Add(author);
        await _db.SaveChangesAsync();
        return new AuthorDto { Id = author.Id, FirstName = author.FirstName, LastName = author.LastName };
    }

    public async Task<(bool found, AuthorDto? result)> UpdateAsync(int id, AuthorCreateDto dto)
    {
        var author = await _db.Authors.FindAsync(id);
        if (author is null) return (false, null);

        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;
        await _db.SaveChangesAsync();

        return (true, new AuthorDto { Id = author.Id, FirstName = author.FirstName, LastName = author.LastName });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _db.Authors.FindAsync(id);
        if (author is null) return false;

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
        return true;
    }
}
