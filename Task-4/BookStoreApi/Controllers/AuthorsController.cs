using BookStoreApi.DTOs.Authors;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authors;

    public AuthorsController(IAuthorService authors) => _authors = authors;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _authors.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var author = await _authors.GetByIdAsync(id);
        if (author is null) return NotFound(new { error = $"Author {id} not found." });
        return Ok(author);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(AuthorCreateDto dto)
    {
        var author = await _authors.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, AuthorCreateDto dto)
    {
        var (found, author) = await _authors.UpdateAsync(id, dto);
        if (!found) return NotFound(new { error = $"Author {id} not found." });
        return Ok(author);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _authors.DeleteAsync(id);
        if (!deleted) return NotFound(new { error = $"Author {id} not found." });
        return NoContent();
    }
}
