// DTOs/CreateProviderGroupDto.cs
namespace Telemed.DTOs;

public class CreateProviderGroupDto
{
    public string GroupName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Speciality { get; set; }
    public string? Website { get; set; }
    public string? Bio { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Zip { get; set; } = null!;
    public string? Country { get; set; }
    public long? CreatedBy { get; set; }
}