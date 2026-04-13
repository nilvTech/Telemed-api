// DTOs/PatientConditionResponseDto.cs
namespace Telemed.DTOs;

public class PatientConditionResponseDto
{
    public long PatientConditionId { get; set; }

    // Patient Info
    public long PatientId { get; set; }
    public string? PatientName { get; set; }
    public string? Mrn { get; set; }

    // Condition Info
    public long ConditionId { get; set; }
    public string? ConditionName { get; set; }
    public string? IcdCode { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }

    // Provider Info
    public long? ProviderInfoId { get; set; }
    public string? ProviderName { get; set; }

    // Consultation Info
    public long? ConsultationId { get; set; }
    public DateTime? ConsultationDate { get; set; }

    // Condition Details
    public string? Status { get; set; }
    public DateTime? OnsetDate { get; set; }
    public string? Note { get; set; }
    public string? ManagedBy { get; set; }

    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}