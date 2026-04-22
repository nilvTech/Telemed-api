// DTOs/CreateFollowupDto.cs
namespace Telemed.DTOs;

public class CreateFollowupDto
{
    public long Patientid { get; set; }
    public long? Appointmentid { get; set; }
    public DateOnly Followupdate { get; set; }
    public string Followuptype { get; set; } = null!;
    public string? Notes { get; set; }
    public string? Status { get; set; }
}