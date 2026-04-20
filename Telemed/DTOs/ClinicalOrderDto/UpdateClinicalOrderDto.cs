// DTOs/UpdateClinicalOrderDto.cs
namespace Telemed.DTOs;

public class UpdateClinicalOrderDto
{
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public DateTime? Completeddate { get; set; }
}