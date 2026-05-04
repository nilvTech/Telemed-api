// DTOs/CreateRoleDto.cs
namespace Telemed.DTOs;

public class CreateRoleDto
{
    public string Rolename { get; set; } = null!;
    public string Rolecode { get; set; } = null!;
    public string Roletype { get; set; } = null!;
    public string Accesslevel { get; set; } = null!;
    public string? Status { get; set; }
    public string? Defaultlandingpage { get; set; }
    public string? Datascope { get; set; }
    public bool? Requiresmfa { get; set; }
    public string? Description { get; set; }
    public long? Createdby { get; set; }
}