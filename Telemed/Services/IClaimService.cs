// Services/Interfaces/IClaimService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IClaimService
{
    // Claim CRUD
    Task<ClaimResponseDto> CreateAsync(CreateClaimDto dto);
    Task<IEnumerable<ClaimResponseDto>> GetAllAsync();
    Task<ClaimResponseDto?> GetByIdAsync(long id);
    Task<ClaimResponseDto?> GetByClaimnumberAsync(string claimnumber);
    Task<IEnumerable<ClaimResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<ClaimResponseDto>> GetByProviderIdAsync(long providerInfoId);
    Task<IEnumerable<ClaimResponseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<ClaimResponseDto>> GetByPayerAsync(string payer);
    Task<ClaimResponseDto?> UpdateAsync(long id, UpdateClaimDto dto);
    Task<ClaimResponseDto?> UpdateStatusAsync(long id, ClaimStatusUpdateDto dto);
    Task<bool> DeleteAsync(long id);

    // ClaimDetail CRUD
    Task<ClaimDetailResponseDto> AddDetailAsync(long claimId, CreateClaimDetailDto dto);
    Task<ClaimDetailResponseDto?> UpdateDetailAsync(long detailId, UpdateClaimDetailDto dto);
    Task<bool> DeleteDetailAsync(long detailId);
}