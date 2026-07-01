using BookStore.Models;
using BookStore.Repositories;

namespace BookStore.Services;

public class BookService
{
    private readonly Repository<Book> _repo;

    public BookService(Repository<Book> repo)
    {
        _repo = repo;
    }

    public void AddBook(Book book) => _repo.Add(book);

    public bool RemoveBook(int id) => _repo.Remove(id);

    public Book? FindById(int id) => _repo.GetById(id);

    public IReadOnlyList<Book> GetAll() => _repo.GetAll();

    public IEnumerable<Book> Search(string term)
        => _repo.Find(b => b.Title.Contains(term, StringComparison.OrdinalIgnoreCase)
                        || b.Author.Contains(term, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Book> FilterByCategory(string category)
        => _repo.Find(b => b.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Book> FilterByAuthor(string author)
        => _repo.Find(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<Book> FilterByPriceRange(decimal min, decimal max)
        => _repo.Find(b => b.Price >= min && b.Price <= max);

    public void ApplyRule(Func<Book, bool> filter, Action<Book> rule)
    {
        var matches = _repo.Find(filter).ToList();
        foreach (var book in matches)
            rule(book);
    }
}
