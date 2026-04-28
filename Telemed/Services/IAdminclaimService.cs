// Services/Interfaces/IAdminclaimService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IAdminclaimService
{
    Task<AdminclaimResponseDto> CreateAsync(CreateAdminclaimDto dto);
    Task<IEnumerable<AdminclaimResponseDto>> GetAllAsync();
    Task<AdminclaimResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<AdminclaimResponseDto>> GetByClaimIdAsync(long claimId);
    Task<IEnumerable<AdminclaimResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<AdminclaimResponseDto>> GetByProviderIdAsync(long providerInfoId);
    Task<IEnumerable<AdminclaimResponseDto>> GetByAppointmentIdAsync(long appointmentId);
    Task<IEnumerable<AdminclaimResponseDto>> GetByEncounterIdAsync(int encounterId);
    Task<IEnumerable<AdminclaimResponseDto>> GetByStatusAsync(string status);
    Task<AdminclaimResponseDto?> UpdateAsync(long id, UpdateAdminclaimDto dto);
    Task<AdminclaimResponseDto?> UpdateStatusAsync(
        long id, AdminclaimStatusUpdateDto dto);
    Task<bool> DeleteAsync(long id);
}