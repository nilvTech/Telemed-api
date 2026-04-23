// DTOs/CarePatientsummaryResponseDto.cs
namespace Telemed.DTOs;

public class CarePatientsummaryResponseDto
{
    // Patient
    public long Patientid { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Fullname { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dateofbirth { get; set; }
    public int? Age { get; set; }

    // Conditions
    public string? Conditions { get; set; }
    public int? Conditioncount { get; set; }
    public List<string>? Conditionlist { get; set; }

    // RPM Latest
    public DateTime? LatestReadingdate { get; set; }
    public int? Systolic { get; set; }
    public int? Diastolic { get; set; }
    public string? Bloodpressure { get; set; }       // Auto "120/80"
    public string? BpStatus { get; set; }            // Normal / High etc
    public int? Heartrate { get; set; }
    public string? HeartrateStatus { get; set; }
    public int? Spo2 { get; set; }
    public string? Spo2Status { get; set; }
    public int? Glucose { get; set; }
    public decimal? Weight { get; set; }
    public bool HasRpmData { get; set; }

    // Alert Latest
    public string? Alerttype { get; set; }
    public bool HasAlert { get; set; }

    // Followup Latest
    public string? Followuptype { get; set; }
    public DateOnly? Followupdate { get; set; }
    public string? FollowupStatus { get; set; }
    public bool IsFollowupOverdue { get; set; }

    // Task Latest
    public string? Taskname { get; set; }
    public DateOnly? Duedate { get; set; }
    public string? TaskStatus { get; set; }
    public bool IsTaskOverdue { get; set; }
    public string? TaskDuedateStatus { get; set; }   // Overdue / Today / Upcoming

    // Careplan Latest
    public string? CareplanStatus { get; set; }
    public bool HasActiveCareplan { get; set; }

    // Smartgoal
    public string? Goaltitle { get; set; }
    public decimal? Progress { get; set; }
    public string? Progresslabel { get; set; }       // "45%"
}