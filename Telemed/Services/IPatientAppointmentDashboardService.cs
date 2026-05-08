// Services/Interfaces/IPatientAppointmentDashboardService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPatientAppointmentDashboardService
{
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetAllAsync();
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetByPatientIdAsync(
        long patientId);
    Task<PatientAppointmentDashboardResponseDto?> GetByAppointmentIdAsync(
        long appointmentId);
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetByProviderIdAsync(
        long providerId);
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetTodayAsync();
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetUpcomingAsync();
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetPastAsync();
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetByStatusAsync(
        string status);
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetByVisitmodeAsync(
        string visitmode);
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetTelemedicineAsync();
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetWithRecordingsAsync();
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> GetByDateRangeAsync(
        DateOnly from, DateOnly to);
    Task<IEnumerable<PatientAppointmentDashboardResponseDto>> SearchAsync(
        string keyword);
}