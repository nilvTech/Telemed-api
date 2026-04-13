// DTOs/ConditionMasterResponseDto.cs
namespace Telemed.DTOs;

public class ConditionMasterResponseDto
{
    public long ConditionId { get; set; }
    public string? ConditionName { get; set; }
    public string? IcdCode { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public int TotalPatients { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}