// DTOs/CreateClaimDetailDto.cs
namespace Telemed.DTOs;

public class CreateClaimDetailDto
{
    public string Cptcode { get; set; } = null!;
    public string? Description { get; set; }
    public int? Units { get; set; }
    public decimal Amount { get; set; }
}