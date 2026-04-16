// DTOs/UpdateRpmmonitoringDto.cs
namespace Telemed.DTOs;

public class UpdateRpmmonitoringDto
{
    public int? Systolic { get; set; }
    public int? Diastolic { get; set; }
    public int? Heartrate { get; set; }
    public int? Spo2 { get; set; }
    public int? Respiratoryrate { get; set; }
    public int? Glucose { get; set; }
    public string? Glucoseunit { get; set; }
    public decimal? Temperature { get; set; }
    public string? Temperatureunit { get; set; }
    public decimal? Weight { get; set; }
    public string? Weightunit { get; set; }
    public decimal? Height { get; set; }
    public string? Heightunit { get; set; }
    public string? Devicetype { get; set; }
    public string? Deviceid { get; set; }
    public string? Note { get; set; }
    public long? Updatedby { get; set; }
}