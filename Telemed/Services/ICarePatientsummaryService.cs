// Services/Interfaces/ICarePatientsummaryService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface ICarePatientsummaryService
{
    Task<IEnumerable<CarePatientsummaryResponseDto>> GetAllAsync();
    Task<CarePatientsummaryResponseDto?> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<CarePatientsummaryResponseDto>> GetByGenderAsync(string gender);
    Task<IEnumerable<CarePatientsummaryResponseDto>> GetByConditionAsync(string condition);
    Task<IEnumerable<CarePatientsummaryResponseDto>> GetWithAlertsAsync();
    Task<IEnumerable<CarePatientsummaryResponseDto>> GetWithActiveCareplanAsync();
    Task<IEnumerable<CarePatientsummaryResponseDto>> GetWithOverdueTasksAsync();
    Task<IEnumerable<CarePatientsummaryResponseDto>> GetWithOverdueFollowupsAsync();
    Task<IEnumerable<CarePatientsummaryResponseDto>> GetByCareplanStatusAsync(string status);
    Task<IEnumerable<CarePatientsummaryResponseDto>> SearchAsync(string keyword);
}