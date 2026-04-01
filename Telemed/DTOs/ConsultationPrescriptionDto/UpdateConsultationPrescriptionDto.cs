// DTOs/UpdateConsultationPrescriptionDto.cs
namespace Telemed.DTOs;

public class UpdateConsultationPrescriptionDto
{
    public string? Medicationname { get; set; }
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }
}