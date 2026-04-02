// DTOs/PatientAlertResponseDto.cs
namespace Telemed.DTOs;

public class PatientAlertResponseDto
{
    public long Alertid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Patientemail { get; set; }
    public string? Mrn { get; set; }

    // Alert Details
    public string? Alerttype { get; set; }
    public string? Alertmessage { get; set; }
    public string? Severity { get; set; }
    public bool Isread { get; set; }
    public bool? Isacknowledged { get; set; }
    public bool Isactive { get; set; }
    public DateTime Createddate { get; set; }
    public DateTime? Updateddate { get; set; }
}