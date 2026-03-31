// DTOs/VitalResponseDto.cs
namespace Telemed.DTOs;

public class VitalResponseDto
{
    public long Vitalsid { get; set; }

    // Encounter Info
    public long Encounterid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public DateTime? Dateofbirth { get; set; }
    public string? Gender { get; set; }

    // Vitals
    public int? Heartrate { get; set; }
    public string? Bloodpressure { get; set; }
    public int? Respiratoryrate { get; set; }
    public decimal? Temperature { get; set; }
    public int? Oxygensaturation { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public decimal? Bmi { get; set; }
    public string? Bmicategory { get; set; }  // Underweight/Normal/Overweight/Obese
    public DateTime? Recordedat { get; set; }
}