// DTOs/UpdatePatientFollowUpDto.cs
namespace Telemed.DTOs;

public class UpdatePatientFollowUpDto
{
    public DateTime? Followupdate { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
}