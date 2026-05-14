// DTOs/ProviderDashboardResponseDto.cs
namespace Telemed.DTOs;

public class ProviderDashboardResponseDto
{
    // Provider Info
    public long Providerinfoid { get; set; }
    public string? Providername { get; set; }
    public string? WelcomeMessage { get; set; }  // "Welcome back, Dr. Emily!"

    // Stats Cards
    public ProviderDashboardStatsDto Stats { get; set; }
        = new ProviderDashboardStatsDto();

    // Today's Appointments list
    public List<ProviderDashboardAppointmentDto> TodaysAppointments { get; set; }
        = new List<ProviderDashboardAppointmentDto>();

    // Notifications
    public List<ProviderDashboardNotificationDto> Notifications { get; set; }
        = new List<ProviderDashboardNotificationDto>();

    // Pending Consultations
    public List<ProviderDashboardConsultationDto> PendingConsultations { get; set; }
        = new List<ProviderDashboardConsultationDto>();

    // Patient Alerts
    public List<ProviderDashboardAlertDto> PatientAlerts { get; set; }
        = new List<ProviderDashboardAlertDto>();
}