// Services/Interfaces/IPatientAlertService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPatientAlertService
{
    Task<PatientAlertResponseDto> CreateAsync(CreatePatientAlertDto dto);
    Task<IEnumerable<PatientAlertResponseDto>> GetAllAsync();
    Task<PatientAlertResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<PatientAlertResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<PatientAlertResponseDto>> GetUnreadByPatientAsync(long patientId);
    Task<IEnumerable<PatientAlertResponseDto>> GetBySeverityAsync(string severity);
    Task<PatientAlertResponseDto?> UpdateAsync(long id, UpdatePatientAlertDto dto);
    Task<PatientAlertResponseDto?> MarkAsReadAsync(long id);
    Task<PatientAlertResponseDto?> AcknowledgeAsync(long id);
    Task<bool> DeleteAsync(long id);
}