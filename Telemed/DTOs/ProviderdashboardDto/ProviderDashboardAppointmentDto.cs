// DTOs/ProviderDashboardAppointmentDto.cs
namespace Telemed.DTOs;

public class ProviderDashboardAppointmentDto
{
    public long Appointmentid { get; set; }
    public string? Patientname { get; set; }
    public string? Visitreason { get; set; }
    public string? Daylabel { get; set; }    // "Today" / "Wednesday" / "Saturday"
    public string? Timerange { get; set; }   // "09:00 - 09:45AM"
    public string? Status { get; set; }
    public string? Visitmode { get; set; }
    public DateOnly? Appointmentdate { get; set; }
    public TimeOnly? Starttime { get; set; }
    public TimeOnly? Endtime { get; set; }
}