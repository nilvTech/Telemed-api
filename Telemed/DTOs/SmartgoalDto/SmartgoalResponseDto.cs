// DTOs/SmartgoalResponseDto.cs
namespace Telemed.DTOs;

public class SmartgoalResponseDto
{
    public long Smartgoalid { get; set; }

    // Careplan Info
    public long Careplanid { get; set; }
    public string? CareplanStatus { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long? Providerinfoid { get; set; }
    public string? Providername { get; set; }

    // Goal Details
    public string? Goaltitle { get; set; }
    public string? Description { get; set; }
    public string? Measurementtype { get; set; }
    public string? MeasurementtypeLabel { get; set; }  // Human readable

    // Values
    public decimal? Targetvalue { get; set; }
    public decimal? Currentvalue { get; set; }
    public string? Unit { get; set; }
    public decimal? Progress { get; set; }
    public string? ProgressLabel { get; set; }         // "45 / 100 lbs"
    public string? Progressstatus { get; set; }        // OnTrack / AtRisk / Behind

    // Dates
    public DateOnly Startdate { get; set; }
    public DateOnly? Targetdate { get; set; }
    public int? DaysRemaining { get; set; }
    public bool IsOverdue { get; set; }

    // Status
    public string? Status { get; set; }

    // Lifestyle
    public string? Diettype { get; set; }
    public string? Exercisetype { get; set; }
    public int? Weeklyminutes { get; set; }

    public string? Notes { get; set; }
    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}