// Models/Carepatientsummary.cs
namespace Telemed.Models;

public partial class Carepatientsummary
{
    public long Patientid { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dateofbirth { get; set; }

    // Conditions
    public string? Conditions { get; set; }

    // RPM Latest
    public DateTime? LatestReadingdate { get; set; }
    public int? Systolic { get; set; }
    public int? Diastolic { get; set; }
    public int? Heartrate { get; set; }
    public int? Spo2 { get; set; }
    public int? Glucose { get; set; }
    public decimal? Weight { get; set; }

    // Alert Latest
    public string? Alerttype { get; set; }

    // Followup Latest
    public string? Followuptype { get; set; }
    public DateOnly? Followupdate { get; set; }
    public string? FollowupStatus { get; set; }

    // Task Latest
    public string? Taskname { get; set; }
    public DateOnly? Duedate { get; set; }
    public string? TaskStatus { get; set; }

    // Careplan Latest
    public string? CareplanStatus { get; set; }

    // Smartgoal
    public string? Goaltitle { get; set; }
    public decimal? Progress { get; set; }
}