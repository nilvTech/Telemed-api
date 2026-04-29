// DTOs/CareteamproviderResponseDto.cs
namespace Telemed.DTOs;

public class CareteamproviderResponseDto
{
    public long Careteamproviderid { get; set; }

    // Careteam Info
    public long Careteamid { get; set; }
    public string? Teamname { get; set; }
    public string? Teamdescription { get; set; }

    // Provider Info
    public long Providerinfoid { get; set; }
    public string? Providername { get; set; }
    public string? Provideremail { get; set; }
    public string? Providerphone { get; set; }
    public string? Speciality1 { get; set; }
    public string? Speciality2 { get; set; }
    public string? Providertype { get; set; }
    public string? NpiNumber { get; set; }

    // Assignment
    public string? Role { get; set; }
    public DateTime? Assigneddate { get; set; }
    public bool? Isactive { get; set; }
    public string? StatusLabel { get; set; }  // "Active" / "Inactive"
}