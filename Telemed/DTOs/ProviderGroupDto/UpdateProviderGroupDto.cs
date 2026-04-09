// DTOs/UpdateProviderGroupDto.cs
namespace Telemed.DTOs;

public class UpdateProviderGroupDto
{
    public string? GroupName { get; set; }
    public string? Phone { get; set; }
    public string? Speciality { get; set; }
    public string? Website { get; set; }
    public string? Bio { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public bool? IsActive { get; set; }
    public long? UpdatedBy { get; set; }
}