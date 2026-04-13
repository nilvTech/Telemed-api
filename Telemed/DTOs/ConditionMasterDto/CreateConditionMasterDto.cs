// DTOs/CreateConditionMasterDto.cs
namespace Telemed.DTOs;

public class CreateConditionMasterDto
{
    public string ConditionName { get; set; } = null!;
    public string IcdCode { get; set; } = null!;
    public string? Description { get; set; }
    public string? Type { get; set; }
}