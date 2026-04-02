// Telemed/Services/Interfaces/IPatientSummaryService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces
{
    public interface IPatientSummaryService
    {
        Task<PatientSummaryDto> GetPatientSummaryAsync();
    }
}