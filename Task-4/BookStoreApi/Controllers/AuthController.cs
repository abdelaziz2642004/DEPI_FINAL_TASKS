using BookStoreApi.DTOs.Auth;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var (success, error, result) = await _auth.RegisterAsync(dto);
        if (!success) return BadRequest(new { error });
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var (success, error, result) = await _auth.LoginAsync(dto);
        if (!success) return Unauthorized(new { error });
        return Ok(result);
    }
}
