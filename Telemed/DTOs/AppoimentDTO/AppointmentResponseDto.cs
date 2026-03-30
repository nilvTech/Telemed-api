// DTOs/AppointmentResponseDto.cs
namespace Telemed.DTOs;

public class AppointmentResponseDto
{
    public int Appointmentid { get; set; }

    // Patient Info
    public int Patientid { get; set; }
    public string? Patientname { get; set; }

    // Provider Info
    public int Providerid { get; set; }
    public string? Providername { get; set; }
    public string? Speciality { get; set; }

    public DateTime Scheduleddatetime { get; set; }
    public string? Status { get; set; }
    public string? Mode { get; set; }
    public string? Notes { get; set; }
    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}