namespace Telemed.DTOs.Auth;

public class RegisterAdminDto
{
    public string AdminName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}