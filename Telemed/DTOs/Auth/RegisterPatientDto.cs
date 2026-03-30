// DTOs/Auth/RegisterPatientDto.cs
namespace Telemed.DTOs.Auth;

public class RegisterPatientDto
{
    public string Firstname { get; set; } = null!;
    public string? Middlename { get; set; }
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public DateTime? Dateofbirth { get; set; }
    public string? Address { get; set; }
}