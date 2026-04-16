// DTOs/ClaimDetailResponseDto.cs
namespace Telemed.DTOs;

public class ClaimDetailResponseDto
{
    public long Claimdetailid { get; set; }
    public long Claimid { get; set; }
    public string? Cptcode { get; set; }
    public string? Description { get; set; }
    public int? Units { get; set; }
    public decimal Amount { get; set; }
}