using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookStoreApi.Data;
using BookStoreApi.DTOs.Auth;
using BookStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreApi.Services;

public interface IAuthService
{
    Task<(bool success, string error, AuthResponseDto? result)> RegisterAsync(RegisterDto dto);
    Task<(bool success, string error, AuthResponseDto? result)> LoginAsync(LoginDto dto);
}

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext db, IConfiguration config, ILogger<AuthService> logger)
    {
        _db = db;
        _config = config;
        _logger = logger;
    }

    public async Task<(bool success, string error, AuthResponseDto? result)> RegisterAsync(RegisterDto dto)
    {
        bool emailTaken = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailTaken)
            return (false, "Email is already registered.", null);

        bool usernameTaken = await _db.Users.AnyAsync(u => u.Username == dto.Username);
        if (usernameTaken)
            return (false, "Username is already taken.", null);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "customer"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _logger.LogInformation("New user registered: {Email}", dto.Email);

        var token = GenerateToken(user);
        return (true, string.Empty, new AuthResponseDto { Token = token, Username = user.Username, Role = user.Role });
    }

    public async Task<(bool success, string error, AuthResponseDto? result)> LoginAsync(LoginDto dto)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for {Email}", dto.Email);
            return (false, "Invalid email or password.", null);
        }

        _logger.LogInformation("User logged in: {Email}", dto.Email);
        var token = GenerateToken(user);
        return (true, string.Empty, new AuthResponseDto { Token = token, Username = user.Username, Role = user.Role });
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
