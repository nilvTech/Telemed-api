// DTOs/CreateAppointmentDto.cs
namespace Telemed.DTOs;

public class CreateAppointmentDto
{
    public int Patientid { get; set; }
    public int Providerid { get; set; }
    public DateTime Scheduleddatetime { get; set; }
    public string? Mode { get; set; }
    public string? Notes { get; set; }
}