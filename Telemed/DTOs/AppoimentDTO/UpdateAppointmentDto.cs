namespace Telemed.DTOs;

public class UpdateAppointmentDto
{
    public DateTime? Scheduleddatetime { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
    public string? Mode { get; set; }
}