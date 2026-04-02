// Services/Interfaces/IPatientFollowUpService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPatientFollowUpService
{
    Task<PatientFollowUpResponseDto> CreateAsync(CreatePatientFollowUpDto dto);
    Task<IEnumerable<PatientFollowUpResponseDto>> GetAllAsync();
    Task<PatientFollowUpResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<PatientFollowUpResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<PatientFollowUpResponseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<PatientFollowUpResponseDto>> GetUpcomingAsync();
    Task<IEnumerable<PatientFollowUpResponseDto>> GetOverdueAsync();
    Task<PatientFollowUpResponseDto?> UpdateAsync(long id, UpdatePatientFollowUpDto dto);
    Task<bool> DeleteAsync(long id);
}