// DTOs/RolesDashboardResponseDto.cs
namespace Telemed.DTOs;

public class RolesDashboardResponseDto
{
    public long Roleid { get; set; }
    public string? Rolename { get; set; }
    public string? Rolecode { get; set; }
    public string? Roletype { get; set; }
    public string? Accesslevel { get; set; }
    public string? Status { get; set; }
    public string? Defaultlandingpage { get; set; }
    public string? Datascope { get; set; }
    public bool? Requiresmfa { get; set; }
    public string? Description { get; set; }
    public long Userscount { get; set; }     // How many users have this role
    public long? Createdby { get; set; }
    public DateTime? Createdat { get; set; }
    public long? Updatedby { get; set; }
    public DateTime? Updatedat { get; set; }
}