// DTOs/AddProviderGroupMemberDto.cs
namespace Telemed.DTOs;

public class AddProviderGroupMemberDto
{
    public long GroupId { get; set; }
    public long ProviderInfoId { get; set; }
    public string? RoleInGroup { get; set; }
    public DateTime? JoinDate { get; set; }
    public long? CreatedBy { get; set; }
}