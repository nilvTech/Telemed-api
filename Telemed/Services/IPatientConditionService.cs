// Services/Interfaces/IPatientConditionService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPatientConditionService
{
    Task<PatientConditionResponseDto> CreateAsync(CreatePatientConditionDto dto);
    Task<IEnumerable<PatientConditionResponseDto>> GetAllAsync();
    Task<PatientConditionResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<PatientConditionResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<PatientConditionResponseDto>> GetActiveByPatientIdAsync(long patientId);
    Task<IEnumerable<PatientConditionResponseDto>> GetByConsultationIdAsync(long consultationId);
    Task<IEnumerable<PatientConditionResponseDto>> GetByProviderIdAsync(long providerInfoId);
    Task<IEnumerable<PatientConditionResponseDto>> GetByStatusAsync(string status);
    Task<PatientConditionResponseDto?> UpdateAsync(long id, UpdatePatientConditionDto dto);
    Task<bool> DeleteAsync(long id);
}