// DTOs/CreatePatientConditionDto.cs
namespace Telemed.DTOs;

public class CreatePatientConditionDto
{
    public long PatientId { get; set; }
    public long ConditionId { get; set; }
    public long? ConsultationId { get; set; }
    public long? ProviderInfoId { get; set; }
    public string? Status { get; set; }
    public DateTime? OnsetDate { get; set; }
    public string? Note { get; set; }
    public string? ManagedBy { get; set; }
    public long? CreatedBy { get; set; }
}