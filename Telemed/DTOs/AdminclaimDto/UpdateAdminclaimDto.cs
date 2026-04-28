// DTOs/UpdateAdminclaimDto.cs
namespace Telemed.DTOs;

public class UpdateAdminclaimDto
{
    public string? Status { get; set; }
    public string? Lastaction { get; set; }
    public DateTime? Lastactiondate { get; set; }
}