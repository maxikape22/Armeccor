using Armeccor.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var user = _context.Usuarios
            .FirstOrDefault(u => u.Username == request.Username);

        if (user == null)
            return Unauthorized("Usuario no encontrado");

        // COMPARACIÓN SIMPLE STRING VS STRING
        if (user.PasswordHash != request.Password)
            return Unauthorized("Contraseña incorrecta");

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("UserId", user.Id.ToString())
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("ESTA_ES_UNA_CLAVE_SUPER_LARGA_Y_SEGURA_123456")
        );

        var token = new JwtSecurityToken(
            issuer: "Armeccor",
            audience: "Armeccor",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}
