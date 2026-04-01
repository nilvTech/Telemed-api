// Services/Interfaces/IAppointmentStatusHistoryService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IAppointmentStatusHistoryService
{
    Task<AppointmentStatusHistoryResponseDto> CreateAsync(
        CreateAppointmentStatusHistoryDto dto);
    Task<IEnumerable<AppointmentStatusHistoryResponseDto>> GetAllAsync();
    Task<AppointmentStatusHistoryResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<AppointmentStatusHistoryResponseDto>> GetByAppointmentIdAsync(
        long appointmentId);
    Task<IEnumerable<AppointmentStatusHistoryResponseDto>> GetByPatientIdAsync(
        long patientId);
    Task<AppointmentStatusHistoryResponseDto?> GetLatestByAppointmentIdAsync(
        long appointmentId);
    Task<bool> DeleteAsync(long id);
}