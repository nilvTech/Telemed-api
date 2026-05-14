// Services/Interfaces/IProviderDashboardService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IProviderDashboardService
{
    // Full dashboard for one provider
    Task<ProviderDashboardResponseDto?> GetDashboardAsync(long providerinfoid);

    // Individual sections (for partial refresh)
    Task<ProviderDashboardStatsDto> GetStatsAsync(long providerinfoid);
    Task<List<ProviderDashboardAppointmentDto>> GetTodaysAppointmentsAsync(
        long providerinfoid);
    Task<List<ProviderDashboardNotificationDto>> GetNotificationsAsync(
        long providerinfoid);
    Task<List<ProviderDashboardConsultationDto>> GetPendingConsultationsAsync(
        long providerinfoid);
    Task<List<ProviderDashboardAlertDto>> GetPatientAlertsAsync(
        long providerinfoid);
}