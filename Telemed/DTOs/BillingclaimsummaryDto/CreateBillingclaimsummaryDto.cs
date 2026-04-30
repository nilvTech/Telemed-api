// DTOs/CreateBillingclaimsummaryDto.cs
namespace Telemed.DTOs;

public class CreateBillingclaimsummaryDto
{
    public long Claimid { get; set; }
    public long Patientid { get; set; }
    public long Providerinfoid { get; set; }
    public string? Cptcode { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = null!;
    public DateTime Claimdate { get; set; }
}