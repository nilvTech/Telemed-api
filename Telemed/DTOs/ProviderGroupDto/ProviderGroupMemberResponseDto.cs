// DTOs/ProviderGroupMemberResponseDto.cs
namespace Telemed.DTOs;

public class ProviderGroupMemberResponseDto
{
    public long GroupMemberId { get; set; }

    // Group Info
    public long GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? GroupEmail { get; set; }
    public string? GroupSpeciality { get; set; }
    public string? GroupState { get; set; }

    // Provider Info
    public long ProviderInfoId { get; set; }
    public string? ProviderFullname { get; set; }
    public string? ProviderEmail { get; set; }
    public string? ProviderPhone { get; set; }
    public string? Speciality1 { get; set; }
    public string? Speciality2 { get; set; }
    public string? Providertype { get; set; }
    public string? NpiNumber { get; set; }

    // Membership Details
    public string? RoleInGroup { get; set; }
    public DateTime? JoinDate { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}