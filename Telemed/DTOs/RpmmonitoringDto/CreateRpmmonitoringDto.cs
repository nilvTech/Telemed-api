// DTOs/CreateRpmmonitoringDto.cs
namespace Telemed.DTOs;

public class CreateRpmmonitoringDto
{
    public long Patientid { get; set; }
    public DateTime Readingdate { get; set; }

    // Blood Pressure
    public int? Systolic { get; set; }
    public int? Diastolic { get; set; }

    // Vitals
    public int? Heartrate { get; set; }
    public int? Spo2 { get; set; }
    public int? Respiratoryrate { get; set; }

    // Glucose
    public int? Glucose { get; set; }
    public string? Glucoseunit { get; set; }

    // Temperature
    public decimal? Temperature { get; set; }
    public string? Temperatureunit { get; set; }

    // Weight
    public decimal? Weight { get; set; }
    public string? Weightunit { get; set; }

    // Height
    public decimal? Height { get; set; }
    public string? Heightunit { get; set; }

    // Device Info
    public string? Devicetype { get; set; }
    public string? Deviceid { get; set; }
    public string? Sourcedata { get; set; }
    public bool? Isauto { get; set; }

    public string? Note { get; set; }
    public long? Createdby { get; set; }
}