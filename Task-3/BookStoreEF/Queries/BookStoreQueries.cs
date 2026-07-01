using BookStoreEF.Data;
using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Queries;

public static class BookStoreQueries
{
    // Task 4 — all books with category and author, single query, no N+1
    public static void ListAllBooks(BookStoreContext db)
    {
        var books = db.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .Include(b => b.Author)
            .OrderBy(b => b.Title)
            .ToList();

        Console.WriteLine("\n=== All Books ===");
        foreach (var b in books)
            Console.WriteLine($"  {b.Title} | {b.Category.CategoryName} | {b.Author.FirstName} {b.Author.LastName} | ${b.Price}");
    }

    // Task 5 — top 5 best-selling books by units sold
    public static void Top5BestSellers(BookStoreContext db)
    {
        var top = db.PurchaseItems
            .AsNoTracking()
            .GroupBy(pi => new { pi.BookId, pi.Book.Title })
            .Select(g => new { g.Key.Title, TotalSold = g.Sum(pi => pi.Quantity) })
            .OrderByDescending(x => x.TotalSold)
            .Take(5)
            .ToList();

        Console.WriteLine("\n=== Top 5 Best Sellers ===");
        foreach (var x in top)
            Console.WriteLine($"  {x.Title} — {x.TotalSold} copies");
    }

    // Task 6 — every customer with their purchase count
    public static void CustomersWithPurchaseCount(BookStoreContext db)
    {
        var result = db.Customers
            .AsNoTracking()
            .Select(c => new
            {
                Name = c.FirstName + " " + c.LastName,
                Count = c.Purchases.Count
            })
            .OrderByDescending(x => x.Count)
            .ToList();

        Console.WriteLine("\n=== Customers & Purchase Count ===");
        foreach (var x in result)
            Console.WriteLine($"  {x.Name} — {x.Count} purchase(s)");
    }

    // Task 7 — categories with more than 5 books
    public static void CategoriesWithMoreThan5Books(BookStoreContext db)
    {
        var result = db.Categories
            .AsNoTracking()
            .Where(c => c.Books.Count > 5)
            .Select(c => new { c.CategoryName, BookCount = c.Books.Count })
            .ToList();

        Console.WriteLine("\n=== Categories with > 5 Books ===");
        foreach (var x in result)
            Console.WriteLine($"  {x.CategoryName} — {x.BookCount} books");
    }

    // Task 8 — books priced above average
    public static void BooksAboveAveragePrice(BookStoreContext db)
    {
        var avg = db.Books.AsNoTracking().Average(b => b.Price);

        var result = db.Books
            .AsNoTracking()
            .Where(b => b.Price > avg)
            .OrderByDescending(b => b.Price)
            .Select(b => new { b.Title, b.Price })
            .ToList();

        Console.WriteLine($"\n=== Books Above Average Price (avg ${avg:F2}) ===");
        foreach (var b in result)
            Console.WriteLine($"  {b.Title} — ${b.Price}");
    }

    // Task 9 — customers who never purchased anything
    public static void CustomersWithNoPurchases(BookStoreContext db)
    {
        var result = db.Customers
            .AsNoTracking()
            .Where(c => !c.Purchases.Any())
            .Select(c => new { c.FirstName, c.LastName, c.Email })
            .ToList();

        Console.WriteLine("\n=== Customers With No Purchases ===");
        foreach (var c in result)
            Console.WriteLine($"  {c.FirstName} {c.LastName} — {c.Email}");
    }

    // Task 10 — total revenue grouped by month
    public static void RevenueByMonth(BookStoreContext db)
    {
        var result = db.Purchases
            .AsNoTracking()
            .SelectMany(p => p.Items, (p, pi) => new { p.PurchaseDate, pi.Quantity, pi.UnitPrice })
            .GroupBy(x => new { x.PurchaseDate.Year, x.PurchaseDate.Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();

        Console.WriteLine("\n=== Revenue by Month ===");
        foreach (var r in result)
            Console.WriteLine($"  {r.Year}-{r.Month:D2} — ${r.Revenue:F2}");
    }

    // Task 11 — search books by keyword (case-insensitive)
    public static void SearchBooks(BookStoreContext db, string keyword)
    {
        var result = db.Books
            .AsNoTracking()
            .Where(b => b.Title.Contains(keyword))
            .Select(b => new { b.Title, b.Price })
            .ToList();

        Console.WriteLine($"\n=== Search: '{keyword}' ===");
        if (result.Count == 0)
            Console.WriteLine("  No results found.");
        else
            foreach (var b in result)
                Console.WriteLine($"  {b.Title} — ${b.Price}");
    }

    // Task 12 — paginated books list
    public static void GetBooksPaged(BookStoreContext db, int page, int pageSize)
    {
        var total = db.Books.AsNoTracking().Count();
        var books = db.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .OrderBy(b => b.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        Console.WriteLine($"\n=== Books (Page {page}, Size {pageSize}, Total {total}) ===");
        foreach (var b in books)
            Console.WriteLine($"  {b.Title} by {b.Author.FirstName} {b.Author.LastName} — ${b.Price}");
    }

    // Task 13 — add a new book, update its price, then delete it
    public static void AddUpdateDelete(BookStoreContext db)
    {
        Console.WriteLine("\n=== Add / Update / Delete Demo ===");

        var category = db.Categories.First();
        var author = db.Authors.First();

        var newBook = new Book
        {
            Title = "Test Book",
            Price = 9.99m,
            StockQuantity = 10,
            CategoryId = category.CategoryId,
            AuthorId = author.AuthorId
        };
        db.Books.Add(newBook);
        db.SaveChanges();
        Console.WriteLine($"  Added: '{newBook.Title}' with ID {newBook.BookId}");

        newBook.Price = 14.99m;
        db.SaveChanges();
        Console.WriteLine($"  Updated price to ${newBook.Price}");

        db.Books.Remove(newBook);
        db.SaveChanges();
        Console.WriteLine($"  Deleted book ID {newBook.BookId}");
    }

    // Task 14 — N+1 already fixed: all queries use Include() or projection
    // Demonstrating the fixed version explicitly here:
    public static void NPlus1Fixed(BookStoreContext db)
    {
        // BAD (N+1 - don't do this):
        // var books = db.Books.ToList();
        // foreach (var b in books)
        //     Console.WriteLine(b.Author.FirstName); // extra DB call per book

        // GOOD — single query with Include:
        var books = db.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToList();

        Console.WriteLine("\n=== N+1 Fixed: Books with Authors (single query) ===");
        foreach (var b in books.Take(5))
            Console.WriteLine($"  {b.Title} — {b.Author.FirstName} {b.Author.LastName}");
    }

    // Task 15 — read-only queries skip change tracking (AsNoTracking used throughout above)
    public static void ReadOnlyQueryDemo(BookStoreContext db)
    {
        var books = db.Books
            .AsNoTracking()
            .OrderBy(b => b.Price)
            .Take(5)
            .ToList();

        Console.WriteLine("\n=== Read-Only (AsNoTracking) — 5 Cheapest Books ===");
        foreach (var b in books)
            Console.WriteLine($"  {b.Title} — ${b.Price}");
    }
}
