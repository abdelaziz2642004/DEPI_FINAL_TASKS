using BookStore.Events;
using BookStore.Models;
using BookStore.Repositories;

namespace BookStore.Services;

public class PurchaseService
{
    private readonly Repository<Purchase> _purchaseRepo;
    private readonly Repository<Book> _bookRepo;

    public event EventHandler<StockEventArgs>? BookOutOfStock;

    public PurchaseService(Repository<Purchase> purchaseRepo, Repository<Book> bookRepo)
    {
        _purchaseRepo = purchaseRepo;
        _bookRepo = bookRepo;
    }

    public (bool ok, string error, Purchase? purchase) RecordPurchase(
        int customerId, string customerName, List<(int bookId, int qty)> items)
    {
        var lineItems = new List<PurchaseItem>();

        foreach (var (bookId, qty) in items)
        {
            var book = _bookRepo.GetById(bookId);
            if (book is null)
                return (false, $"Book ID {bookId} does not exist.", null);

            if (book.Stock < qty)
                return (false, $"Not enough stock for '{book.Title}'. Only {book.Stock} left.", null);
        }

        foreach (var (bookId, qty) in items)
        {
            var book = _bookRepo.GetById(bookId)!;
            book.Stock -= qty;

            lineItems.Add(new PurchaseItem
            {
                BookId = book.Id,
                BookTitle = book.Title,
                Quantity = qty,
                UnitPrice = book.Price
            });

            if (book.Stock == 0)
                BookOutOfStock?.Invoke(this, new StockEventArgs(book.Id, book.Title));
        }

        var purchase = new Purchase
        {
            CustomerId = customerId,
            CustomerName = customerName,
            Date = DateTime.Now,
            Items = lineItems
        };

        _purchaseRepo.Add(purchase);
        return (true, string.Empty, purchase);
    }

    public IEnumerable<Purchase> GetByCustomer(int customerId)
        => _purchaseRepo.Find(p => p.CustomerId == customerId);

    public IReadOnlyList<Purchase> GetAll() => _purchaseRepo.GetAll();
}
