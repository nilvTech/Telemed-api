// DTOs/UpdateFollowupDto.cs
namespace Telemed.DTOs;

public class UpdateFollowupDto
{
    public DateOnly? Followupdate { get; set; }
    public string? Followuptype { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
}