// DTOs/ClaimStatusUpdateDto.cs
namespace Telemed.DTOs;

public class ClaimStatusUpdateDto
{
    public string Status { get; set; } = null!;
    public decimal? Paidamount { get; set; }
    public string? Deniedreason { get; set; }
    public long? Updatedby { get; set; }
}