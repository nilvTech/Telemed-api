// DTOs/Auth/RegisterProviderDto.cs
namespace Telemed.DTOs.Auth;

public class RegisterProviderDto
{
    public string Providername { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Speciality { get; set; }
    public string? Website { get; set; }
    public string? Primaryaddress { get; set; }
}