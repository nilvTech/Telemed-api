// Services/Interfaces/ICareplanService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface ICareplanService
{
    Task<CareplanResponseDto> CreateAsync(CreateCareplanDto dto);
    Task<IEnumerable<CareplanResponseDto>> GetAllAsync();
    Task<CareplanResponseDto?> GetByIdAsync(long id);

    // Patient-focused queries
    Task<IEnumerable<CareplanResponseDto>> GetByPatientIdAsync(long patientId);
    Task<CareplanResponseDto?> GetActiveByPatientIdAsync(long patientId);

    // Provider-focused queries
    Task<IEnumerable<CareplanResponseDto>> GetByProviderIdAsync(long providerInfoId);

    // Clinical filtering
    Task<IEnumerable<CareplanResponseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<CareplanResponseDto>> GetByRisklevelAsync(string risklevel);
    Task<IEnumerable<CareplanResponseDto>> GetOverdueForReviewAsync();
    Task<IEnumerable<CareplanResponseDto>> GetDueForReviewTodayAsync();

    // CCM (Chronic Care Management)
    Task<IEnumerable<CareplanResponseDto>> GetCcmNotMetAsync();
    Task<CareplanResponseDto?> AddCcmMinutesAsync(
        long id, int minutes, long? updatedby);

    // Update
    Task<CareplanResponseDto?> UpdateAsync(long id, UpdateCareplanDto dto);
    Task<CareplanResponseDto?> UpdateStatusAsync(
        long id, CareplanStatusUpdateDto dto);

    Task<bool> DeleteAsync(long id);
}