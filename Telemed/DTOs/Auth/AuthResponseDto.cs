// DTOs/Auth/AuthResponseDto.cs
namespace Telemed.DTOs.Auth;

public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string Role { get; set; } = null!;
    public int UserId { get; set; }
    public int ReferenceId { get; set; }
    public string Email { get; set; } = null!;
    public string Fullname { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}