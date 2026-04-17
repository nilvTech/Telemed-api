// Services/Interfaces/ISmartgoalService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface ISmartgoalService
{
    Task<SmartgoalResponseDto> CreateAsync(CreateSmartgoalDto dto);
    Task<IEnumerable<SmartgoalResponseDto>> GetAllAsync();
    Task<SmartgoalResponseDto?> GetByIdAsync(long id);

    // Patient focused
    Task<IEnumerable<SmartgoalResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<SmartgoalResponseDto>> GetActiveByPatientIdAsync(long patientId);

    // Careplan focused
    Task<IEnumerable<SmartgoalResponseDto>> GetByCareplanIdAsync(long careplanId);

    // Provider focused
    Task<IEnumerable<SmartgoalResponseDto>> GetByProviderIdAsync(long providerInfoId);

    // Clinical filtering
    Task<IEnumerable<SmartgoalResponseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<SmartgoalResponseDto>> GetByMeasurementtypeAsync(string measurementtype);
    Task<IEnumerable<SmartgoalResponseDto>> GetOverdueAsync();
    Task<IEnumerable<SmartgoalResponseDto>> GetAtRiskAsync();

    // Progress update
    Task<SmartgoalResponseDto?> UpdateProgressAsync(
        long id, SmartgoalProgressUpdateDto dto);

    Task<SmartgoalResponseDto?> UpdateAsync(long id, UpdateSmartgoalDto dto);
    Task<bool> DeleteAsync(long id);
}