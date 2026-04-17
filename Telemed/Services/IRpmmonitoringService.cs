using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IRpmmonitoringService
{
    Task<RpmmonitoringResponseDto> CreateAsync(CreateRpmmonitoringDto dto);
    Task<IEnumerable<RpmmonitoringResponseDto>> GetAllAsync();
    Task<RpmmonitoringResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<RpmmonitoringResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<RpmmonitoringResponseDto>> GetByPatientIdAndDateRangeAsync(
        long patientId, DateTime from, DateTime to);
    Task<IEnumerable<RpmmonitoringResponseDto>> GetUnreviewedAsync();
    Task<IEnumerable<RpmmonitoringResponseDto>> GetUnreviewedByPatientAsync(
        long patientId);
    Task<RpmmonitoringResponseDto?> GetLatestByPatientAsync(long patientId);
    Task<RpmmonitoringResponseDto?> UpdateAsync(long id, UpdateRpmmonitoringDto dto);
    Task<RpmmonitoringResponseDto?> MarkReviewedAsync(long id, RpmReviewDto dto);
    Task<bool> DeleteAsync(long id);
}