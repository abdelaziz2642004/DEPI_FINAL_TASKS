using System.Security.Claims;
using BookStoreApi.Data;
using BookStoreApi.DTOs.Orders;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.Services;

public interface IOrderService
{
    Task<(bool success, string error, OrderDto? order)> PlaceOrderAsync(int userId, PlaceOrderDto dto);
    Task<List<OrderDto>> GetMyOrdersAsync(int userId);
    Task<List<OrderDto>> GetAllOrdersAsync();
    Task<OrderDto?> GetByIdAsync(int id, int userId, string role);
}

public class OrderService : IOrderService
{
    private readonly AppDbContext _db;
    private readonly ILogger<OrderService> _logger;

    public OrderService(AppDbContext db, ILogger<OrderService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<(bool success, string error, OrderDto? order)> PlaceOrderAsync(int userId, PlaceOrderDto dto)
    {
        var order = new Order { UserId = userId, Items = new List<OrderItem>() };

        foreach (var item in dto.Items)
        {
            var book = await _db.Books.FindAsync(item.BookId);
            if (book is null)
                return (false, $"Book with ID {item.BookId} not found.", null);

            if (book.Stock < item.Quantity)
                return (false, $"'{book.Title}' only has {book.Stock} in stock.", null);

            book.Stock -= item.Quantity;

            order.Items.Add(new OrderItem
            {
                BookId = book.Id,
                Quantity = item.Quantity,
                UnitPrice = book.Price
            });
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Order #{OrderId} placed by user {UserId}", order.Id, userId);

        return (true, string.Empty, await GetOrderDtoAsync(order.Id));
    }

    public async Task<List<OrderDto>> GetMyOrdersAsync(int userId)
    {
        return await _db.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.Items).ThenInclude(i => i.Book)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => MapOrder(o))
            .ToListAsync();
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        return await _db.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.Items).ThenInclude(i => i.Book)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => MapOrder(o))
            .ToListAsync();
    }

    public async Task<OrderDto?> GetByIdAsync(int id, int userId, string role)
    {
        var order = await _db.Orders
            .AsNoTracking()
            .Include(o => o.User)
            .Include(o => o.Items).ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null) return null;
        if (role != "admin" && order.UserId != userId) return null;

        return MapOrder(order);
    }

    private async Task<OrderDto?> GetOrderDtoAsync(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.Items).ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(o => o.Id == id);

        return order is null ? null : MapOrder(order);
    }

    private static OrderDto MapOrder(Order o) => new()
    {
        Id = o.Id,
        OrderDate = o.OrderDate,
        Status = o.Status,
        CustomerName = o.User?.Username ?? string.Empty,
        Items = o.Items.Select(i => new OrderItemDto
        {
            BookId = i.BookId,
            BookTitle = i.Book?.Title ?? string.Empty,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList()
    };
}
