// Services/Interfaces/IAppointmentNoteService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IAppointmentNoteService
{
    Task<AppointmentNoteResponseDto> CreateAsync(CreateAppointmentNoteDto dto);
    Task<IEnumerable<AppointmentNoteResponseDto>> GetAllAsync();
    Task<AppointmentNoteResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<AppointmentNoteResponseDto>> GetByAppointmentIdAsync(long appointmentId);
    Task<IEnumerable<AppointmentNoteResponseDto>> GetByPatientIdAsync(long patientId);
    Task<AppointmentNoteResponseDto?> UpdateAsync(long id, UpdateAppointmentNoteDto dto);
    Task<bool> DeleteAsync(long id);
}