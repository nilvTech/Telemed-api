// DTOs/AppointmentStatusUpdateDto.cs
namespace Telemed.DTOs;

public class AppointmentStatusUpdateDto
{
    public string Status { get; set; } = null!;
    public long? Updatedby { get; set; }
}