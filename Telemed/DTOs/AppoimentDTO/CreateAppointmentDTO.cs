// DTOs/CreateAppointmentDto.cs
namespace Telemed.DTOs;

public class CreateAppointmentDto
{
    public long Patientid { get; set; }
    public long Providerid { get; set; }
    public long? Clinicid { get; set; }
    public DateOnly Appointmentdate { get; set; }
    public TimeOnly Starttime { get; set; }
    public TimeOnly? Endtime { get; set; }
    public string Visittype { get; set; } = null!;
    public string Visitmode { get; set; } = null!;
    public string? Visitreason { get; set; }
    public string? Priority { get; set; }
    public bool Isfollowup { get; set; } = false;
    public long? Parentappointmentid { get; set; }
    public string? Meetinglink { get; set; }
    public string? Meetingid { get; set; }
    public long? Createdby { get; set; }
}