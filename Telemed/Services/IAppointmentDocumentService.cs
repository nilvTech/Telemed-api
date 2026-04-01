// Services/Interfaces/IAppointmentDocumentService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IAppointmentDocumentService
{
    Task<AppointmentDocumentResponseDto> CreateAsync(
        CreateAppointmentDocumentDto dto);
    Task<IEnumerable<AppointmentDocumentResponseDto>> GetAllAsync();
    Task<AppointmentDocumentResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<AppointmentDocumentResponseDto>> GetByAppointmentIdAsync(
        long appointmentId);
    Task<IEnumerable<AppointmentDocumentResponseDto>> GetByPatientIdAsync(
        long patientId);
    Task<IEnumerable<AppointmentDocumentResponseDto>> GetByFileTypeAsync(
        string filetype);
    Task<AppointmentDocumentResponseDto?> UpdateAsync(
        long id, UpdateAppointmentDocumentDto dto);
    Task<bool> DeleteAsync(long id);
}