// DTOs/CreatePatientFollowUpDto.cs
namespace Telemed.DTOs;

public class CreatePatientFollowUpDto
{
    public long Patientid { get; set; }
    public DateTime Followupdate { get; set; }
    public string? Notes { get; set; }
}