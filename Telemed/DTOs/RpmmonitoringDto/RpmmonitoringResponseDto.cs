// DTOs/RpmmonitoringResponseDto.cs
namespace Telemed.DTOs;

public class RpmmonitoringResponseDto
{
    public long Rpmmonitoringid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }
    public string? Gender { get; set; }

    // Reading Date
    public DateTime Readingdate { get; set; }

    // Blood Pressure
    public int? Systolic { get; set; }
    public int? Diastolic { get; set; }
    public string? Bloodpressure { get; set; }      // Auto formatted "120/80"
    public string? Bpstatus { get; set; }           // Normal / Elevated / High

    // Vitals
    public int? Heartrate { get; set; }
    public string? Heartratestatus { get; set; }    // Normal / High / Low
    public int? Spo2 { get; set; }
    public string? Spo2status { get; set; }         // Normal / Low / Critical
    public int? Respiratoryrate { get; set; }

    // Glucose
    public int? Glucose { get; set; }
    public string? Glucoseunit { get; set; }
    public string? Glucosestatus { get; set; }      // Normal / High / Low

    // Temperature
    public decimal? Temperature { get; set; }
    public string? Temperatureunit { get; set; }
    public string? Temperaturestatus { get; set; }  // Normal / Fever / Hypothermia

    // Weight & Height
    public decimal? Weight { get; set; }
    public string? Weightunit { get; set; }
    public decimal? Height { get; set; }
    public string? Heightunit { get; set; }
    public decimal? Bmi { get; set; }               // Auto calculated
    public string? Bmistatus { get; set; }          // Underweight/Normal/Overweight/Obese

    // Device Info
    public string? Devicetype { get; set; }
    public string? Deviceid { get; set; }
    public string? Sourcedata { get; set; }
    public bool? Isauto { get; set; }

    // Review Info
    public bool? Isreviewed { get; set; }
    public DateTime? Reviewedat { get; set; }
    public long? Reviewedby { get; set; }
    public string? Reviewedbyname { get; set; }

    public string? Note { get; set; }
    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}