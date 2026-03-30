// Services/Interfaces/IAppointmentService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto);
    Task<IEnumerable<AppointmentResponseDto>> GetAllAsync();
    Task<AppointmentResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<AppointmentResponseDto>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<AppointmentResponseDto>> GetByProviderIdAsync(int providerId);
    Task<AppointmentResponseDto?> UpdateAsync(int id, UpdateAppointmentDto dto);
    Task<bool> DeleteAsync(int id);
}