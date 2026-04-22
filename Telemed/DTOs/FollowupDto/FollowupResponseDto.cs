// DTOs/FollowupResponseDto.cs
namespace Telemed.DTOs;

public class FollowupResponseDto
{
    public long Followupid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Appointment Info
    public long? Appointmentid { get; set; }
    public DateOnly? Appointmentdate { get; set; }
    public string? Appointmentstatus { get; set; }

    // Followup Details
    public DateOnly Followupdate { get; set; }
    public string? DuedateStatus { get; set; }   // Overdue / Today / Upcoming
    public int? DaysUntilFollowup { get; set; }
    public string? Followuptype { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }
}