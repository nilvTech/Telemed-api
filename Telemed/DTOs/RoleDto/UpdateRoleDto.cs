// DTOs/UpdateRoleDto.cs
namespace Telemed.DTOs;

public class UpdateRoleDto
{
    public string? Rolename { get; set; }
    public string? Roletype { get; set; }
    public string? Accesslevel { get; set; }
    public string? Status { get; set; }
    public string? Defaultlandingpage { get; set; }
    public string? Datascope { get; set; }
    public bool? Requiresmfa { get; set; }
    public string? Description { get; set; }
    public long? Updatedby { get; set; }
}