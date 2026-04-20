// DTOs/ClinicalOrderStatusUpdateDto.cs
namespace Telemed.DTOs;

public class ClinicalOrderStatusUpdateDto
{
    public string Status { get; set; } = null!;
    public DateTime? Completeddate { get; set; }
}