// DTOs/UpdatePatientAlertDto.cs
namespace Telemed.DTOs;

public class UpdatePatientAlertDto
{
    public string? Alerttype { get; set; }
    public string? Alertmessage { get; set; }
    public string? Severity { get; set; }
    public bool? Isread { get; set; }
    public bool? Isacknowledged { get; set; }
    public bool? Isactive { get; set; }
}