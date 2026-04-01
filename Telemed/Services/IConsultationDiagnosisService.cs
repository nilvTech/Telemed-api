// Services/Interfaces/IConsultationDiagnosisService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IConsultationDiagnosisService
{
    Task<ConsultationDiagnosisResponseDto> CreateAsync(
        CreateConsultationDiagnosisDto dto);
    Task<IEnumerable<ConsultationDiagnosisResponseDto>> GetAllAsync();
    Task<ConsultationDiagnosisResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<ConsultationDiagnosisResponseDto>> GetByConsultationIdAsync(
        long consultationId);
    Task<IEnumerable<ConsultationDiagnosisResponseDto>> GetByPatientIdAsync(
        long patientId);
    Task<IEnumerable<ConsultationDiagnosisResponseDto>> GetByDiagnosisCodeAsync(
        string code);
    Task<ConsultationDiagnosisResponseDto?> UpdateAsync(
        long id, UpdateConsultationDiagnosisDto dto);
    Task<bool> DeleteAsync(long id);
}