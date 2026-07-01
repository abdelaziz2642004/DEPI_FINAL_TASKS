using System.Security.Claims;
using BookStoreApi.DTOs.Orders;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orders;

    public OrdersController(IOrderService orders) => _orders = orders;

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private string GetRole() => User.FindFirstValue(ClaimTypes.Role) ?? "customer";

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
    {
        var (success, error, order) = await _orders.PlaceOrderAsync(GetUserId(), dto);
        if (!success) return BadRequest(new { error });
        return CreatedAtAction(nameof(GetById), new { id = order!.Id }, order);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders()
    {
        var orders = await _orders.GetMyOrdersAsync(GetUserId());
        return Ok(orders);
    }

    [HttpGet("all")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orders.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orders.GetByIdAsync(id, GetUserId(), GetRole());
        if (order is null) return NotFound(new { error = $"Order {id} not found." });
        return Ok(order);
    }
}
