// DTOs/PrescriptionResponseDto.cs
namespace Telemed.DTOs;

public class PrescriptionResponseDto
{
    public long Prescriptionid { get; set; }

    // Encounter Info
    public long Encounterid { get; set; }
    public string? Diagnosis { get; set; }
    public string? Icd10code { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public DateTime? Dateofbirth { get; set; }
    public string? Gender { get; set; }

    // Provider Info
    public long Providerid { get; set; }
    public string? Providername { get; set; }
    public string? Speciality { get; set; }

    // Prescription Details
    public string Medicinename { get; set; } = null!;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Notes { get; set; }
    public DateTime? Createdat { get; set; }
}