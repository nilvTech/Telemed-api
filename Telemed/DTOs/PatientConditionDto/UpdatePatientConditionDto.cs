// DTOs/UpdatePatientConditionDto.cs
namespace Telemed.DTOs;

public class UpdatePatientConditionDto
{
    public string? Status { get; set; }
    public DateTime? OnsetDate { get; set; }
    public string? Note { get; set; }
    public string? ManagedBy { get; set; }
    public long? ProviderInfoId { get; set; }
    public long? UpdatedBy { get; set; }
}