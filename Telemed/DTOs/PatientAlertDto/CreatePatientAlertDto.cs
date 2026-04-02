// DTOs/CreatePatientAlertDto.cs
namespace Telemed.DTOs;

public class CreatePatientAlertDto
{
    public long Patientid { get; set; }
    public string Alerttype { get; set; } = null!;
    public string Alertmessage { get; set; } = null!;
    public string? Severity { get; set; }
}