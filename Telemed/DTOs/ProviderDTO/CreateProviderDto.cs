namespace Telemed.DTOs;

public class CreateProviderDto
{
    public string ProviderName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Speciality { get; set; }
    public string? Website { get; set; }
    public string? PrimaryAddress { get; set; }
    public string? Status { get; set; } = "Active";  // Active / Inactive
}