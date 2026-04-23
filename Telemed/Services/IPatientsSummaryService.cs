// Services/Interfaces/IPatientSummaryService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPatientsSummaryService
{
    Task<IEnumerable<PatientsSummaryResponseDto>> GetAllAsync();
    Task<PatientsSummaryResponseDto?> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<PatientsSummaryResponseDto>> GetByGenderAsync(string gender);
    Task<IEnumerable<PatientsSummaryResponseDto>> GetByConditionAsync(string condition);
    Task<IEnumerable<PatientsSummaryResponseDto>> SearchAsync(string keyword);
}