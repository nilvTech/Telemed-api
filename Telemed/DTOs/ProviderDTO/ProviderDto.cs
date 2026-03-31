namespace Telemed.DTOs;

public class ProviderDto
{
    public long ProviderId { get; set; }
    public string? ProviderName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Speciality { get; set; }
    public string? Website { get; set; }
    public string? PrimaryAddress { get; set; }
    public string? Status { get; set; }
}