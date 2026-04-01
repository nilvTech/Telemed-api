// DTOs/CreateConsultationPrescriptionDto.cs
namespace Telemed.DTOs;

public class CreateConsultationPrescriptionDto
{
    public long Consultationid { get; set; }
    public string Medicationname { get; set; } = null!;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }
}