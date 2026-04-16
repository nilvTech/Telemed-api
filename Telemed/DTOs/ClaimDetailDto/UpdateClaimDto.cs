// DTOs/UpdateClaimDto.cs
namespace Telemed.DTOs;

public class UpdateClaimDto
{
    public string? Payer { get; set; }
    public int? Totaltime { get; set; }
    public string? Icdcode { get; set; }
    public string? Status { get; set; }
    public DateOnly? Submissiondate { get; set; }
    public decimal? Paidamount { get; set; }
    public string? Deniedreason { get; set; }
    public long? Updatedby { get; set; }
}