using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.DTOs.Categories;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BookCount { get; set; }
}

public class CategoryCreateDto
{
    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;
}
