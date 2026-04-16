// DTOs/ClaimResponseDto.cs
namespace Telemed.DTOs;

public class ClaimResponseDto
{
    public long Claimid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long Providerinfoid { get; set; }
    public string? Providername { get; set; }

    // Claim Info
    public string? Payer { get; set; }
    public string? Claimnumber { get; set; }
    public int? Totaltime { get; set; }
    public decimal Totalamount { get; set; }
    public string? Status { get; set; }
    public DateOnly? Submissiondate { get; set; }
    public decimal? Paidamount { get; set; }
    public decimal? Balancedue { get; set; }  // Auto calculated
    public string? Deniedreason { get; set; }
    public string? Icdcode { get; set; }

    // Details
    public List<ClaimDetailResponseDto> Claimdetails { get; set; }
        = new List<ClaimDetailResponseDto>();

    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}