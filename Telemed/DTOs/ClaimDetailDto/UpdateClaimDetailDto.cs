// DTOs/UpdateClaimDetailDto.cs
namespace Telemed.DTOs;

public class UpdateClaimDetailDto
{
    public string? Cptcode { get; set; }
    public string? Description { get; set; }
    public int? Units { get; set; }
    public decimal? Amount { get; set; }
}