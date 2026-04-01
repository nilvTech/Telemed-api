// DTOs/UpdateAppointmentDto.cs
namespace Telemed.DTOs;

public class UpdateAppointmentDto
{
    public DateOnly? Appointmentdate { get; set; }
    public TimeOnly? Starttime { get; set; }
    public TimeOnly? Endtime { get; set; }
    public string? Visittype { get; set; }
    public string? Visitmode { get; set; }
    public string? Visitreason { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public int? Tokennumber { get; set; }
    public int? Queueposition { get; set; }
    public DateTime? Checkedintime { get; set; }
    public int? Waitingminutes { get; set; }
    public string? Meetinglink { get; set; }
    public string? Meetingid { get; set; }
    public bool? Ispaid { get; set; }
    public long? Paymentid { get; set; }
    public bool? Isfollowup { get; set; }
    public bool? Isactive { get; set; }
    public long? Updatedby { get; set; }
}