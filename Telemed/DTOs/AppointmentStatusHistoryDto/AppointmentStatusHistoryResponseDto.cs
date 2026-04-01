// DTOs/AppointmentStatusHistoryResponseDto.cs
namespace Telemed.DTOs;

public class AppointmentStatusHistoryResponseDto
{
    public long Id { get; set; }
    public long? Appointmentid { get; set; }

    // Appointment Info
    public DateOnly? Appointmentdate { get; set; }
    public string? Visittype { get; set; }

    // Patient Info
    public long? Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long? Providerid { get; set; }
    public string? Providername { get; set; }

    // Status History Details
    public string? Status { get; set; }
    public DateTime? Changedat { get; set; }
    public long? Changedby { get; set; }
}