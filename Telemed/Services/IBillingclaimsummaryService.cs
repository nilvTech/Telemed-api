// Services/Interfaces/IBillingclaimsummaryService.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Services.Interfaces;

public interface IBillingclaimsummaryService
{
    Task<BillingclaimsummaryResponseDto> CreateAsync(
        CreateBillingclaimsummaryDto dto);
    Task<IEnumerable<BillingclaimsummaryResponseDto>> GetAllAsync();
    Task<BillingclaimsummaryResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByClaimIdAsync(long claimId);
    Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByProviderIdAsync(long providerInfoId);
    Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByCptcodeAsync(string cptcode);
    Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByDateRangeAsync(
        DateTime from, DateTime to);
    Task<BillingSummaryStatsDto> GetStatsAsync();
    Task<BillingSummaryStatsDto> GetStatsByProviderAsync(long providerInfoId);
    Task<BillingSummaryStatsDto> GetStatsByPatientAsync(long patientId);
    Task<BillingclaimsummaryResponseDto?> UpdateAsync(
        long id, UpdateBillingclaimsummaryDto dto);
    Task<BillingclaimsummaryResponseDto?> UpdateStatusAsync(
        long id, BillingStatusUpdateDto dto);
    Task<bool> DeleteAsync(long id);

    // bill View


}