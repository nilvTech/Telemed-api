// Services/Interfaces/IClaimformService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IClaimformService
{
    Task<ClaimformResponseDto> CreateAsync(CreateClaimformDto dto);
    Task<IEnumerable<ClaimformResponseDto>> GetAllAsync();
    Task<ClaimformResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<ClaimformResponseDto>> GetByPatientnameAsync(string patientname);
    Task<IEnumerable<ClaimformResponseDto>> GetByPolicynumberAsync(string policynumber);
    Task<IEnumerable<ClaimformResponseDto>> GetByDiagnosiscodeAsync(string diagnosiscode);
    Task<IEnumerable<ClaimformResponseDto>> GetByServicedateAsync(DateOnly servicedate);
    Task<IEnumerable<ClaimformResponseDto>> GetByDateRangeAsync(
        DateOnly from, DateOnly to);
    Task<IEnumerable<ClaimformResponseDto>> SearchAsync(string keyword);
    Task<ClaimformResponseDto?> UpdateAsync(long id, UpdateClaimformDto dto);
    Task<bool> DeleteAsync(long id);
}