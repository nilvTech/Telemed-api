// DTOs/CreateSmartgoalDto.cs
namespace Telemed.DTOs;

public class CreateSmartgoalDto
{
    public long Careplanid { get; set; }
    public long Patientid { get; set; }
    public long? Providerinfoid { get; set; }
    public string Goaltitle { get; set; } = null!;
    public string? Description { get; set; }
    public string Measurementtype { get; set; } = null!;
    public decimal? Targetvalue { get; set; }
    public decimal? Currentvalue { get; set; }
    public string? Unit { get; set; }
    public DateOnly Startdate { get; set; }
    public DateOnly? Targetdate { get; set; }
    public string? Status { get; set; }
    public string? Diettype { get; set; }
    public string? Exercisetype { get; set; }
    public int? Weeklyminutes { get; set; }
    public string? Notes { get; set; }
    public long? Createdby { get; set; }
}