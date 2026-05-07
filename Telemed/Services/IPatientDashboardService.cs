// Services/Interfaces/IPatientDashboardService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPatientDashboardService
{
    Task<IEnumerable<PatientDashboardResponseDto>> GetAllAsync();
    Task<PatientDashboardResponseDto?> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<PatientDashboardResponseDto>> GetWithAppointmentsTodayAsync();
    Task<IEnumerable<PatientDashboardResponseDto>> GetWithPendingClaimsAsync();
    Task<IEnumerable<PatientDashboardResponseDto>> GetWithUnreadNotificationsAsync();
    Task<IEnumerable<PatientDashboardResponseDto>> GetWithActiveVideoSessionsAsync();
    Task<IEnumerable<PatientDashboardResponseDto>> SearchAsync(string keyword);
}