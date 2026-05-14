// DTOs/ProviderDashboardConsultationDto.cs
namespace Telemed.DTOs;

public class ProviderDashboardConsultationDto
{
    public long Consultationid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }
    public int? Waitingminutes { get; set; }   // "Waiting 5 min"
    public string? Waitinglabel { get; set; }  // "Waiting 5 min"
    public string? Status { get; set; }
    public DateTime? Starttime { get; set; }
}