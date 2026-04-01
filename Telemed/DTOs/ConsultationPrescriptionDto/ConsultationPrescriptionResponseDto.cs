// DTOs/ConsultationPrescriptionResponseDto.cs
namespace Telemed.DTOs;

public class ConsultationPrescriptionResponseDto
{
    public long Prescriptionid { get; set; }
    public long Consultationid { get; set; }

    // Consultation Info
    public string? Consultationstatus { get; set; }
    public DateTime? Consultationstarttime { get; set; }

    // Patient Info
    public long? Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long? Providerid { get; set; }
    public string? Providername { get; set; }
    public string? Speciality { get; set; }

    // Prescription Details
    public string? Medicationname { get; set; }
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }
}