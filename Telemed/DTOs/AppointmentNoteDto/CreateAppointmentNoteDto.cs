// DTOs/CreateAppointmentNoteDto.cs
namespace Telemed.DTOs;

public class CreateAppointmentNoteDto
{
    public long Appointmentid { get; set; }
    public string Notes { get; set; } = null!;
}