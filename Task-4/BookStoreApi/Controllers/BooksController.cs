using BookStoreApi.DTOs.Books;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _books;

    public BooksController(IBookService books) => _books = books;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BookQueryDto query)
    {
        var result = await _books.GetBooksAsync(query);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await _books.GetByIdAsync(id);
        if (book is null) return NotFound(new { error = $"Book {id} not found." });
        return Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(BookCreateDto dto)
    {
        var book = await _books.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, BookUpdateDto dto)
    {
        var (found, book) = await _books.UpdateAsync(id, dto);
        if (!found) return NotFound(new { error = $"Book {id} not found." });
        return Ok(book);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var (found, conflict) = await _books.DeleteAsync(id);
        if (!found) return NotFound(new { error = $"Book {id} not found." });
        if (conflict) return Conflict(new { error = "Cannot delete this book because it has existing orders." });
        return NoContent();
    }
}
