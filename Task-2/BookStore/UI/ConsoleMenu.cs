using BookStore.Events;
using BookStore.Extensions;
using BookStore.Models;
using BookStore.Services;

namespace BookStore.UI;

public class ConsoleMenu
{
    private readonly BookService _books;
    private readonly CustomerService _customers;
    private readonly PurchaseService _purchases;
    private readonly AnalyticsService _analytics;

    public ConsoleMenu(
        BookService books,
        CustomerService customers,
        PurchaseService purchases,
        AnalyticsService analytics)
    {
        _books = books;
        _customers = customers;
        _purchases = purchases;
        _analytics = analytics;

        _purchases.BookOutOfStock += OnBookOutOfStock;
    }

    private static void OnBookOutOfStock(object? sender, StockEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n  [!] '{e.Title}' is now out of stock!");
        Console.ResetColor();
    }

    public Task RunAsync()
    {
        while (true)
        {
            Clear();
            Header("BookStore Manager");
            Console.WriteLine("  [1] Books");
            Console.WriteLine("  [2] Customers");
            Console.WriteLine("  [3] Purchases");
            Console.WriteLine("  [4] Analytics");
            Console.WriteLine("  [5] Filter Books");
            Console.WriteLine("  [6] Apply Rule to Books");
            Console.WriteLine("  [0] Exit");
            Console.Write("\n  > ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": BooksMenu(); break;
                case "2": CustomersMenu(); break;
                case "3": PurchasesMenu(); break;
                case "4": ShowAnalytics(); break;
                case "5": FilterMenu(); break;
                case "6": ApplyRuleMenu(); break;
                case "0": return Task.CompletedTask;
                default: Err("Unknown option."); break;
            }
        }
    }

    // ─── Books ────────────────────────────────────────────────

    private void BooksMenu()
    {
        while (true)
        {
            Clear();
            Header("Books");
            Console.WriteLine("  [1] Add Book");
            Console.WriteLine("  [2] Remove Book");
            Console.WriteLine("  [3] Search");
            Console.WriteLine("  [4] List All");
            Console.WriteLine("  [0] Back");
            Console.Write("\n  > ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": AddBook(); break;
                case "2": RemoveBook(); break;
                case "3": SearchBooks(); break;
                case "4":
                    Clear();
                    Header("All Books");
                    PrintBooks(_books.GetAll());
                    Pause();
                    break;
                case "0": return;
                default: Err("Unknown option."); break;
            }
        }
    }

    private void AddBook()
    {
        Clear();
        Header("Add Book");

        Console.WriteLine("  Format:  1=Paperback   2=Ebook   3=Audiobook");
        Console.Write("  Format > ");
        string fmt = Console.ReadLine()?.Trim() ?? "1";

        string title = Ask("Title");
        if (title.IsBlank()) { Err("Title cannot be empty."); return; }

        string author = Ask("Author");
        if (author.IsBlank()) { Err("Author cannot be empty."); return; }

        string category = Ask("Category");
        if (category.IsBlank()) { Err("Category cannot be empty."); return; }

        decimal price = Ask("Price").ToDecimal(-1);
        if (price <= 0) { Err("Price must be greater than zero."); return; }

        int stock = Ask("Stock Quantity").ToInt(-1);
        if (stock < 0) { Err("Stock cannot be negative."); return; }

        Book book = fmt switch
        {
            "2" => BuildEbook(title, author, category, price, stock),
            "3" => BuildAudiobook(title, author, category, price, stock),
            _ => BuildPaperback(title, author, category, price, stock)
        };

        _books.AddBook(book);
        Ok($"Added '{book.Title}' (ID {book.Id}).");
    }

    private PaperbackBook BuildPaperback(string title, string author, string category, decimal price, int stock)
    {
        int pages = Ask("Page Count").ToInt(0);
        return new PaperbackBook
        {
            Title = title, Author = author, Category = category,
            Price = price, Stock = stock, PageCount = pages
        };
    }

    private Ebook BuildEbook(string title, string author, string category, decimal price, int stock)
    {
        string fileFmt = Ask("File Format (PDF/EPUB/MOBI)");
        return new Ebook
        {
            Title = title, Author = author, Category = category,
            Price = price, Stock = stock,
            FileFormat = fileFmt.IsBlank() ? "PDF" : fileFmt.ToUpper()
        };
    }

    private Audiobook BuildAudiobook(string title, string author, string category, decimal price, int stock)
    {
        double hours = Ask("Duration in hours").ToDouble(0);
        return new Audiobook
        {
            Title = title, Author = author, Category = category,
            Price = price, Stock = stock, DurationHours = hours
        };
    }

    private void RemoveBook()
    {
        Clear();
        Header("Remove Book");
        PrintBooks(_books.GetAll());

        int id = Ask("\n  Book ID to remove").ToInt(-1);
        if (id < 1) { Err("Invalid ID."); return; }

        if (_books.RemoveBook(id)) Ok("Book removed.");
        else Err($"No book found with ID {id}.");
    }

    private void SearchBooks()
    {
        Clear();
        Header("Search Books");
        string term = Ask("Search (title or author)");
        if (term.IsBlank()) { Err("Enter a search term."); return; }

        var results = _books.Search(term).ToList();
        Console.WriteLine();
        if (results.Count == 0) Console.WriteLine("  No matches found.");
        else PrintBooks(results);
        Pause();
    }

    // ─── Customers ────────────────────────────────────────────

    private void CustomersMenu()
    {
        while (true)
        {
            Clear();
            Header("Customers");
            Console.WriteLine("  [1] Register Customer");
            Console.WriteLine("  [2] List All");
            Console.WriteLine("  [0] Back");
            Console.Write("\n  > ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": RegisterCustomer(); break;
                case "2":
                    Clear();
                    Header("All Customers");
                    var all = _customers.GetAll();
                    if (all.Count == 0) Console.WriteLine("  No customers yet.");
                    else foreach (var c in all) Console.WriteLine($"  {c}");
                    Pause();
                    break;
                case "0": return;
                default: Err("Unknown option."); break;
            }
        }
    }

    private void RegisterCustomer()
    {
        Clear();
        Header("Register Customer");

        string first = Ask("First Name");
        if (first.IsBlank()) { Err("First name is required."); return; }

        string last = Ask("Last Name");
        if (last.IsBlank()) { Err("Last name is required."); return; }

        string email = Ask("Email");
        if (email.IsBlank() || !email.Contains('@'))
        {
            Err("Please enter a valid email address.");
            return;
        }

        var customer = new Customer { FirstName = first, LastName = last, Email = email };
        var (ok, error) = _customers.Register(customer);

        if (ok) Ok($"Registered '{customer.FullName}' (ID {customer.Id}).");
        else Err(error);
    }

    // ─── Purchases ────────────────────────────────────────────

    private void PurchasesMenu()
    {
        while (true)
        {
            Clear();
            Header("Purchases");
            Console.WriteLine("  [1] Record Purchase");
            Console.WriteLine("  [2] View All Purchases");
            Console.WriteLine("  [3] Customer Purchase History");
            Console.WriteLine("  [0] Back");
            Console.Write("\n  > ");

            switch (Console.ReadLine()?.Trim())
            {
                case "1": RecordPurchase(); break;
                case "2": ListAllPurchases(); break;
                case "3": ViewCustomerHistory(); break;
                case "0": return;
                default: Err("Unknown option."); break;
            }
        }
    }

    private void RecordPurchase()
    {
        Clear();
        Header("Record Purchase");

        var customers = _customers.GetAll();
        if (customers.Count == 0) { Err("No customers registered yet."); return; }

        Console.WriteLine();
        foreach (var c in customers) Console.WriteLine($"  {c}");
        Console.WriteLine();

        int custId = Ask("Customer ID").ToInt(-1);
        var customer = _customers.FindById(custId);
        if (customer is null) { Err("Customer not found."); return; }

        var inStock = _books.GetAll().Where(b => b.Stock > 0).ToList();
        if (inStock.Count == 0) { Err("No books currently in stock."); return; }

        Console.WriteLine();
        PrintBooks(inStock);

        var items = new List<(int bookId, int qty)>();

        while (true)
        {
            Console.WriteLine();
            int bookId = Ask("Book ID to add (0 = done)").ToInt(0);
            if (bookId == 0) break;

            var book = _books.FindById(bookId);
            if (book is null) { Console.WriteLine("  Book not found, try again."); continue; }
            if (book.Stock == 0) { Console.WriteLine($"  '{book.Title}' is out of stock."); continue; }

            int qty = Ask($"Quantity (available: {book.Stock})").ToInt(0);
            if (qty <= 0) { Console.WriteLine("  Quantity must be at least 1."); continue; }
            if (qty > book.Stock) { Console.WriteLine($"  Only {book.Stock} available."); continue; }

            items.Add((bookId, qty));
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  Added: {book.Title} x{qty}");
            Console.ResetColor();
        }

        if (items.Count == 0) { Err("No items added. Purchase cancelled."); return; }

        var (ok, error, purchase) = _purchases.RecordPurchase(custId, customer.FullName, items);
        if (ok && purchase is not null)
            Ok($"Purchase #{purchase.Id} recorded. Total: {purchase.Total.ToPrice()}");
        else
            Err(error);
    }

    private void ListAllPurchases()
    {
        Clear();
        Header("All Purchases");

        var all = _purchases.GetAll();
        if (all.Count == 0) { Console.WriteLine("  No purchases yet."); Pause(); return; }

        foreach (var p in all)
        {
            Console.WriteLine($"\n  Purchase #{p.Id} | {p.CustomerName} | {p.Date:g}");
            foreach (var item in p.Items)
                Console.WriteLine($"    {item.BookTitle} x{item.Quantity} @ {item.UnitPrice.ToPrice()} = {item.LineTotal.ToPrice()}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    Total: {p.Total.ToPrice()}");
            Console.ResetColor();
        }
        Pause();
    }

    private void ViewCustomerHistory()
    {
        Clear();
        Header("Customer Purchase History");

        int id = Ask("Customer ID").ToInt(-1);
        var customer = _customers.FindById(id);
        if (customer is null) { Err("Customer not found."); return; }

        var history = _purchases.GetByCustomer(id).ToList();
        if (history.Count == 0)
        {
            Console.WriteLine($"\n  {customer.FullName} has no purchases on record.");
            Pause();
            return;
        }

        Console.WriteLine($"\n  History for {customer.FullName}:\n");
        foreach (var p in history)
        {
            Console.WriteLine($"  Purchase #{p.Id} | {p.Date:g}");
            foreach (var item in p.Items)
                Console.WriteLine($"    {item.BookTitle} x{item.Quantity} = {item.LineTotal.ToPrice()}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    Total: {p.Total.ToPrice()}");
            Console.ResetColor();
        }
        Pause();
    }

    // ─── Analytics ────────────────────────────────────────────

    private void ShowAnalytics()
    {
        Clear();
        Header("Analytics");

        decimal revenue = _analytics.TotalRevenue();
        var bestBook = _analytics.BestSellingBook();
        var (topCust, topSpent) = _analytics.TopCustomer();

        Console.WriteLine($"  Total Revenue    : {revenue.ToPrice()}");
        Console.WriteLine($"  Best-Selling Book: {(bestBook is null ? "N/A" : bestBook.Title)}");
        Console.WriteLine($"  Top Customer     : {(topCust is null ? "N/A" : $"{topCust.FullName} ({topSpent.ToPrice()})")}");

        Pause();
    }

    // ─── Filters ──────────────────────────────────────────────

    private void FilterMenu()
    {
        Clear();
        Header("Filter Books");
        Console.WriteLine("  [1] By Category");
        Console.WriteLine("  [2] By Author");
        Console.WriteLine("  [3] By Price Range");
        Console.WriteLine("  [0] Back");
        Console.Write("\n  > ");

        List<Book> results;
        switch (Console.ReadLine()?.Trim())
        {
            case "1":
                string cat = Ask("Category");
                results = _books.FilterByCategory(cat).ToList();
                Console.WriteLine();
                PrintBooks(results);
                Pause();
                break;

            case "2":
                string author = Ask("Author name");
                results = _books.FilterByAuthor(author).ToList();
                Console.WriteLine();
                PrintBooks(results);
                Pause();
                break;

            case "3":
                decimal min = Ask("Min price").ToDecimal(0);
                decimal max = Ask("Max price").ToDecimal(decimal.MaxValue);
                results = _books.FilterByPriceRange(min, max).ToList();
                Console.WriteLine();
                PrintBooks(results);
                Pause();
                break;
        }
    }

    // ─── Apply Rule ───────────────────────────────────────────

    private void ApplyRuleMenu()
    {
        Clear();
        Header("Apply Rule to Books");
        Console.WriteLine("  [1] Apply % discount to a category");
        Console.WriteLine("  [2] Restock books below a minimum");
        Console.WriteLine("  [0] Back");
        Console.Write("\n  > ");

        switch (Console.ReadLine()?.Trim())
        {
            case "1":
                string cat = Ask("Category");
                int pct = Ask("Discount %").ToInt(-1);
                if (pct < 1 || pct > 99) { Err("Discount must be between 1 and 99."); return; }

                _books.ApplyRule(
                    b => b.Category.Equals(cat, StringComparison.OrdinalIgnoreCase),
                    b => b.Price = b.Price.Discounted(pct)
                );
                Ok($"Applied {pct}% discount to all '{cat}' books.");
                break;

            case "2":
                int minStock = Ask("Minimum stock level").ToInt(-1);
                if (minStock < 0) { Err("Stock cannot be negative."); return; }

                _books.ApplyRule(
                    b => b.Stock < minStock,
                    b => b.Stock = minStock
                );
                Ok($"All books below {minStock} in stock have been restocked.");
                break;
        }
    }

    // ─── Helpers ──────────────────────────────────────────────

    private static void PrintBooks(IEnumerable<Book> books)
    {
        var list = books.ToList();
        if (list.Count == 0) { Console.WriteLine("  No books found."); return; }
        foreach (var b in list)
            Console.WriteLine($"  {b.GetDetails()}");
    }

    private static string Ask(string label)
    {
        Console.Write($"  {label}: ");
        return Console.ReadLine() ?? string.Empty;
    }

    private static void Header(string title)
    {
        Console.WriteLine($"  ══════════════════════════════");
        Console.WriteLine($"   {title}");
        Console.WriteLine($"  ══════════════════════════════\n");
    }

    private static void Err(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n  Error: {message}");
        Console.ResetColor();
        Pause();
    }

    private static void Ok(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n  {message}");
        Console.ResetColor();
        Pause();
    }

    private static void Pause()
    {
        Console.Write("\n  Press Enter to continue...");
        Console.ReadLine();
    }

    private static void Clear()
    {
        try
        {
            Console.Clear();
        }
        catch (System.IO.IOException)
        {
        }
    }
}

