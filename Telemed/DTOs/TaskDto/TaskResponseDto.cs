// DTOs/TaskResponseDto.cs
namespace Telemed.DTOs;

public class TaskResponseDto
{
    public long Taskid { get; set; }
    public string? Taskname { get; set; }
    public DateOnly Duedate { get; set; }
    public string? DuedateStatus { get; set; }  // Overdue / DueToday / Upcoming

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long? Providerinfoid { get; set; }
    public string? Providername { get; set; }

    // Task Details
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? Description { get; set; }
    public DateOnly? Completeddate { get; set; }

    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}