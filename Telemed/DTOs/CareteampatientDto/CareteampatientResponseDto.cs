// DTOs/CareteampatientResponseDto.cs
namespace Telemed.DTOs;

public class CareteampatientResponseDto
{
    public long Careteampatientid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dateofbirth { get; set; }
    public int? Age { get; set; }

    // Careteam Info
    public long Careteamid { get; set; }
    public string? Teamname { get; set; }
    public string? Teamdescription { get; set; }

    // Assignment
    public DateTime? Assigneddate { get; set; }
    public bool? Isactive { get; set; }
    public string? StatusLabel { get; set; }  // "Active" / "Inactive"
}