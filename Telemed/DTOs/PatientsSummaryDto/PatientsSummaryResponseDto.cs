// DTOs/PatientSummaryResponseDto.cs
namespace Telemed.DTOs;

public class PatientsSummaryResponseDto
{
    public long Patientid { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Fullname { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dateofbirth { get; set; }
    public int? Age { get; set; }               // Auto calculated
    public string? Conditions { get; set; }
    public int? Conditioncount { get; set; }    // Auto calculated
    public List<string>? Conditionlist { get; set; } // Split into list
}