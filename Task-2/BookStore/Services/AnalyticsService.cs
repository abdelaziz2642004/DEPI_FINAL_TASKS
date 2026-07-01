using BookStore.Models;
using BookStore.Repositories;

namespace BookStore.Services;

public class AnalyticsService
{
    private readonly Repository<Purchase> _purchaseRepo;
    private readonly Repository<Book> _bookRepo;
    private readonly Repository<Customer> _customerRepo;

    public AnalyticsService(
        Repository<Purchase> purchaseRepo,
        Repository<Book> bookRepo,
        Repository<Customer> customerRepo)
    {
        _purchaseRepo = purchaseRepo;
        _bookRepo = bookRepo;
        _customerRepo = customerRepo;
    }

    public decimal TotalRevenue()
        => _purchaseRepo.GetAll().Sum(p => p.Total);

    public Book? BestSellingBook()
    {
        var topSale = _purchaseRepo.GetAll()
            .SelectMany(p => p.Items)
            .GroupBy(i => i.BookId)
            .Select(g => new { BookId = g.Key, UnitsSold = g.Sum(i => i.Quantity) })
            .OrderByDescending(x => x.UnitsSold)
            .FirstOrDefault();

        return topSale is null ? null : _bookRepo.GetById(topSale.BookId);
    }

    public (Customer? customer, decimal totalSpent) TopCustomer()
    {
        var top = _purchaseRepo.GetAll()
            .GroupBy(p => p.CustomerId)
            .Select(g => new { CustomerId = g.Key, Spent = g.Sum(p => p.Total) })
            .OrderByDescending(x => x.Spent)
            .FirstOrDefault();

        if (top is null) return (null, 0);
        return (_customerRepo.GetById(top.CustomerId), top.Spent);
    }
}
