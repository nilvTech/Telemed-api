// DTOs/UpdateBillingclaimsummaryDto.cs
namespace Telemed.DTOs;

public class UpdateBillingclaimsummaryDto
{
    public string? Cptcode { get; set; }
    public decimal? Amount { get; set; }
    public string? Status { get; set; }
    public DateTime? Claimdate { get; set; }
}