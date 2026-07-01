using BookStoreApi.DTOs.Categories;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categories;

    public CategoriesController(ICategoryService categories) => _categories = categories;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _categories.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cat = await _categories.GetByIdAsync(id);
        if (cat is null) return NotFound(new { error = $"Category {id} not found." });
        return Ok(cat);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        var cat = await _categories.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = cat.Id }, cat);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, CategoryCreateDto dto)
    {
        var (found, cat) = await _categories.UpdateAsync(id, dto);
        if (!found) return NotFound(new { error = $"Category {id} not found." });
        return Ok(cat);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _categories.DeleteAsync(id);
        if (!deleted) return NotFound(new { error = $"Category {id} not found." });
        return NoContent();
    }
}
