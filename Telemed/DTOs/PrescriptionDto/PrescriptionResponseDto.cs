// DTOs/PrescriptionResponseDto.cs
namespace Telemed.DTOs;

public class PrescriptionResponseDto
{
    public int Prescriptionid { get; set; }

    // Encounter Info
    public int Encounterid { get; set; }
    public string? Diagnosis { get; set; }
    public string? Icd10code { get; set; }

    // Patient Info
    public int Patientid { get; set; }
    public string? Patientname { get; set; }
    public DateTime? Dateofbirth { get; set; }
    public string? Gender { get; set; }

    // Provider Info
    public int Providerid { get; set; }
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