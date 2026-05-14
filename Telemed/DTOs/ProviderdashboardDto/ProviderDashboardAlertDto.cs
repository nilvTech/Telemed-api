// DTOs/ProviderDashboardAlertDto.cs
namespace Telemed.DTOs;

public class ProviderDashboardAlertDto
{
    public long Alertid { get; set; }
    public string? Patientname { get; set; }
    public string? Alertmessage { get; set; }
    public string? Alerttype { get; set; }
    public string? Severity { get; set; }
    public bool? Isacknowledged { get; set; }
    public DateTime? Createddate { get; set; }
}