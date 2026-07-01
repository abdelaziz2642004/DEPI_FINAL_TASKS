namespace BookStoreEF.Models;

public class Author
{
    public int AuthorId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public ICollection<Book> Books { get; set; } = new List<Book>();
}
