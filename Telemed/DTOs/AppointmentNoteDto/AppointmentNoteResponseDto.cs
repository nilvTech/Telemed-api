// DTOs/AppointmentNoteResponseDto.cs
namespace Telemed.DTOs;

public class AppointmentNoteResponseDto
{
    public long Id { get; set; }
    public long? Appointmentid { get; set; }

    // Appointment Info
    public DateOnly? Appointmentdate { get; set; }
    public string? Visittype { get; set; }
    public string? Status { get; set; }

    // Patient Info
    public long? Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long? Providerid { get; set; }
    public string? Providername { get; set; }

    // Note Details
    public string? Notes { get; set; }
    public DateTime? Createdat { get; set; }
}