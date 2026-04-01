// Services/Interfaces/IAppointmentService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto);
    Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
    Task<AppointmentResponseDto?> GetByIdAsync(long id);
    Task<AppointmentResponseDto?> UpdateAsync(long id, UpdateAppointmentDto dto);
    Task<bool> DeleteAsync(long id);
    Task<IEnumerable<AppointmentResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<AppointmentResponseDto>> GetByProviderIdAsync(long providerId);
    Task<IEnumerable<AppointmentResponseDto>> GetByDateAsync(DateOnly date);
    Task<IEnumerable<AppointmentResponseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<AppointmentResponseDto>> GetTodayAsync();
    Task<IEnumerable<AppointmentResponseDto>> GetUpcomingByPatientAsync(long patientId);
    Task<IEnumerable<AppointmentResponseDto>> GetUpcomingByProviderAsync(long providerId);
    Task<AppointmentResponseDto?> UpdateStatusAsync(long id, AppointmentStatusUpdateDto dto);
    Task<AppointmentResponseDto?> CheckInAsync(long id, CheckInDto dto);
    Task<IEnumerable<AppointmentResponseDto>> GetFollowUpsAsync(long patientId);
}