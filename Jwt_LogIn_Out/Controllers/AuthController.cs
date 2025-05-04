using Jwt_LogIn_Out.Models;
using Jwt_LogIn_Out.Data;
using Jwt_LogIn_Out.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(AppDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        user.Sifre = HashPassword(user.Sifre);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok("Kayıt başarılı");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(User dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.KullaniciAdi == dto.KullaniciAdi);
        if (user == null || user.Sifre != HashPassword(dto.Sifre))
            return Unauthorized("Hatalı kullanıcı adı veya şifre");

        var token = _tokenService.CreateToken(user);
        return Ok(new { token });
    }

    private static string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
