// DTOs/AppointmentDocumentResponseDto.cs
namespace Telemed.DTOs;

public class AppointmentDocumentResponseDto
{
    public long Id { get; set; }
    public long? Appointmentid { get; set; }

    // Appointment Info
    public DateOnly? Appointmentdate { get; set; }
    public string? Visittype { get; set; }
    public string? Visitreason { get; set; }

    // Patient Info
    public long? Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long? Providerid { get; set; }
    public string? Providername { get; set; }

    // Document Details
    public string? Fileurl { get; set; }
    public string? Filetype { get; set; }
    public string? Filename { get; set; }  // Extracted from URL
    public DateTime? Uploadedat { get; set; }
}