// DTOs/PatientFollowUpResponseDto.cs
namespace Telemed.DTOs;

public class PatientFollowUpResponseDto
{
    public long Id { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Patientemail { get; set; }
    public string? Mrn { get; set; }

    // FollowUp Details
    public DateTime Followupdate { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
    public DateTime? Createdat { get; set; }
}