using BookStoreApi.Data;
using BookStoreApi.DTOs.Books;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Services;

public interface IBookService
{
    Task<PagedResult<BookDto>> GetBooksAsync(BookQueryDto query);
    Task<BookDto?> GetByIdAsync(int id);
    Task<BookDto> CreateAsync(BookCreateDto dto);
    Task<(bool found, BookDto? book)> UpdateAsync(int id, BookUpdateDto dto);
    Task<(bool found, bool conflict)> DeleteAsync(int id);
}

public class BookService : IBookService
{
    private readonly AppDbContext _db;

    public BookService(AppDbContext db) => _db = db;

    public async Task<PagedResult<BookDto>> GetBooksAsync(BookQueryDto query)
    {
        var q = _db.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .Include(b => b.Author)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
            q = q.Where(b => b.Title.Contains(query.Search));

        if (query.CategoryId.HasValue)
            q = q.Where(b => b.CategoryId == query.CategoryId.Value);

        if (query.AuthorId.HasValue)
            q = q.Where(b => b.AuthorId == query.AuthorId.Value);

        if (query.MinPrice.HasValue)
            q = q.Where(b => b.Price >= query.MinPrice.Value);

        if (query.MaxPrice.HasValue)
            q = q.Where(b => b.Price <= query.MaxPrice.Value);

        int total = await q.CountAsync();

        var books = await q
            .OrderBy(b => b.Title)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(b => ToDto(b))
            .ToListAsync();

        return new PagedResult<BookDto>
        {
            Items = books,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var b = await _db.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        return b is null ? null : ToDto(b);
    }

    public async Task<BookDto> CreateAsync(BookCreateDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            AuthorId = dto.AuthorId
        };
        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        await _db.Entry(book).Reference(b => b.Category).LoadAsync();
        await _db.Entry(book).Reference(b => b.Author).LoadAsync();

        return ToDto(book);
    }

    public async Task<(bool found, BookDto? book)> UpdateAsync(int id, BookUpdateDto dto)
    {
        var book = await _db.Books
            .Include(b => b.Category)
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return (false, null);

        if (dto.Title is not null) book.Title = dto.Title;
        if (dto.Price.HasValue) book.Price = dto.Price.Value;
        if (dto.Stock.HasValue) book.Stock = dto.Stock.Value;
        if (dto.CategoryId.HasValue) book.CategoryId = dto.CategoryId.Value;
        if (dto.AuthorId.HasValue) book.AuthorId = dto.AuthorId.Value;

        await _db.SaveChangesAsync();

        await _db.Entry(book).Reference(b => b.Category).LoadAsync();
        await _db.Entry(book).Reference(b => b.Author).LoadAsync();

        return (true, ToDto(book));
    }

    public async Task<(bool found, bool conflict)> DeleteAsync(int id)
    {
        var book = await _db.Books.FindAsync(id);
        if (book is null) return (false, false);

        bool hasOrders = await _db.OrderItems.AnyAsync(oi => oi.BookId == id);
        if (hasOrders) return (true, true);

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return (true, false);
    }

    private static BookDto ToDto(Book b) => new()
    {
        Id = b.Id,
        Title = b.Title,
        Price = b.Price,
        Stock = b.Stock,
        CategoryId = b.CategoryId,
        AuthorId = b.AuthorId,
        CategoryName = b.Category?.Name ?? string.Empty,
        AuthorName = b.Author is null ? string.Empty : $"{b.Author.FirstName} {b.Author.LastName}"
    };
}
