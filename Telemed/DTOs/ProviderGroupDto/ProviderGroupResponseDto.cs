// DTOs/ProviderGroupResponseDto.cs
namespace Telemed.DTOs;

public class ProviderGroupResponseDto
{
    public long GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? Email { get; set; }
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
    public string? FullAddress { get; set; }
    public bool? IsActive { get; set; }
    public int TotalMembers { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Members list
    public List<ProviderGroupMemberSummaryDto> Members { get; set; }
        = new List<ProviderGroupMemberSummaryDto>();
}