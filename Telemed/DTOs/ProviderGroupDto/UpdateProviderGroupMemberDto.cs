// DTOs/UpdateProviderGroupMemberDto.cs
namespace Telemed.DTOs;

public class UpdateProviderGroupMemberDto
{
    public string? RoleInGroup { get; set; }
    public bool? IsActive { get; set; }
    public long? UpdatedBy { get; set; }
}