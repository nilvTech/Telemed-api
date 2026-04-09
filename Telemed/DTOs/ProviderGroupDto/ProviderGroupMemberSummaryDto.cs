// DTOs/ProviderGroupMemberSummaryDto.cs
namespace Telemed.DTOs;

public class ProviderGroupMemberSummaryDto
{
    public long GroupMemberId { get; set; }
    public long ProviderInfoId { get; set; }
    public string? ProviderFullname { get; set; }
    public string? ProviderEmail { get; set; }
    public string? ProviderPhone { get; set; }
    public string? Speciality1 { get; set; }
    public string? Speciality2 { get; set; }
    public string? Providertype { get; set; }
    public string? NpiNumber { get; set; }
    public string? RoleInGroup { get; set; }
    public DateTime? JoinDate { get; set; }
    public bool? IsActive { get; set; }
}