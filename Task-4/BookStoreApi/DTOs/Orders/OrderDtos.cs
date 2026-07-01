using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.DTOs.Orders;

public class OrderItemDto
{
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.LineTotal);
}

public class PlaceOrderItemDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int BookId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}

public class PlaceOrderDto
{
    [Required]
    [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<PlaceOrderItemDto> Items { get; set; } = new();
}
