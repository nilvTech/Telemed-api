// DTOs/BillingclaimsummaryResponseDto.cs
namespace Telemed.DTOs;

public class BillingclaimsummaryResponseDto
{
    public long Summaryid { get; set; }

    // Claim Info
    public long Claimid { get; set; }
    public string? Claimnumber { get; set; }
    public string? Claimpayer { get; set; }
    public decimal? Claimtotalamount { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long Providerinfoid { get; set; }
    public string? Providername { get; set; }
    public string? Providerspeciality { get; set; }
    public string? NpiNumber { get; set; }

    // Billing Details
    public string? Cptcode { get; set; }
    public decimal Amount { get; set; }
    public string? Status { get; set; }
    public DateTime Claimdate { get; set; }

    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}