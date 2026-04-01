// DTOs/CreateAppointmentStatusHistoryDto.cs
namespace Telemed.DTOs;

public class CreateAppointmentStatusHistoryDto
{
    public long Appointmentid { get; set; }
    public string Status { get; set; } = null!;
    public long? Changedby { get; set; }
}