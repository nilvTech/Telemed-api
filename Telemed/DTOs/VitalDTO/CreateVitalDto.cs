// DTOs/CreateVitalDto.cs
namespace Telemed.DTOs;

public class CreateVitalDto
{
    public int Encounterid { get; set; }
    public int Patientid { get; set; }
    public int? Heartrate { get; set; }
    public string? Bloodpressure { get; set; }
    public int? Respiratoryrate { get; set; }
    public decimal? Temperature { get; set; }
    public int? Oxygensaturation { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Bmi { get; set; }
}