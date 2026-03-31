using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Telemed.Models;

namespace Telemed.Services;

public class JwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Generates JWT Token with clean and consistent claims
    /// </summary>
    public string GenerateToken(User user, string fullname)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!.Trim()));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Revised Claims - Using short names + standard ClaimTypes (Recommended)
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Userid.ToString()),
            new Claim(ClaimTypes.Email, user.Email.Trim()),
            new Claim(ClaimTypes.Role, user.Role.Trim()),           // This is important
            new Claim("ReferenceId", user.Referenceid.ToString()),
            new Claim("Fullname", fullname.Trim())
        };

        var expiry = DateTime.UtcNow.AddMinutes(
            double.Parse(_config["JwtSettings:ExpiryMinutes"]!));

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"]!.Trim(),
            audience: _config["JwtSettings:Audience"]!.Trim(),
            claims: claims,
            expires: expiry,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    // Optional: Keep this for manual token validation if needed elsewhere
    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!.Trim());

        try
        {
            return tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _config["JwtSettings:Issuer"]!.Trim(),
                ValidateAudience = true,
                ValidAudience = _config["JwtSettings:Audience"]!.Trim(),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.NameIdentifier
            }, out _);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Token validation error: {ex.Message}");
            return null;
        }
    }
}