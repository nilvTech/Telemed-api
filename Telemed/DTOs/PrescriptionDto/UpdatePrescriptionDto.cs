// DTOs/UpdatePrescriptionDto.cs
namespace Telemed.DTOs;

public class UpdatePrescriptionDto
{
    public string? Medicinename { get; set; }
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Notes { get; set; }
}