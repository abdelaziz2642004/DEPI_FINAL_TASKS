using System.Text.Json.Serialization;

namespace BookStore.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(PaperbackBook), "paperback")]
[JsonDerivedType(typeof(Ebook), "ebook")]
[JsonDerivedType(typeof(Audiobook), "audiobook")]
public abstract class Book : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    [JsonIgnore]
    public abstract string Format { get; }

    public abstract string GetDetails();

    public override string ToString()
        => $"[{Id}] {Title} by {Author} | {Category} | {Format} | ${Price:F2} | Stock: {Stock}";
}
