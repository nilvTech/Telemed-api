// DTOs/CreatePrescriptionDto.cs
namespace Telemed.DTOs;

public class CreatePrescriptionDto
{
    public int Encounterid { get; set; }
    public long Patientid { get; set; }
    public long Providerid { get; set; }
    public string Medicinename { get; set; } = null!;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Notes { get; set; }
}