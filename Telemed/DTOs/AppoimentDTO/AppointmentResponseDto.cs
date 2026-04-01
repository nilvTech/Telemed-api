// DTOs/AppointmentResponseDto.cs
namespace Telemed.DTOs;

public class AppointmentResponseDto
{
    public long Appointmentid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Patientemail { get; set; }
    public string? Patientphone { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long Providerid { get; set; }
    public string? Providername { get; set; }
    public string? Speciality { get; set; }
    public string? Provideremail { get; set; }

    // Appointment Details
    public long? Clinicid { get; set; }
    public DateOnly Appointmentdate { get; set; }
    public TimeOnly Starttime { get; set; }
    public TimeOnly? Endtime { get; set; }
    public string? Visittype { get; set; }
    public string? Visitmode { get; set; }
    public string? Visitreason { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }

    // Queue Info
    public int? Tokennumber { get; set; }
    public int? Queueposition { get; set; }
    public DateTime? Checkedintime { get; set; }
    public int? Waitingminutes { get; set; }

    // Telehealth Info
    public string? Meetinglink { get; set; }
    public string? Meetingid { get; set; }

    // Payment Info
    public bool Ispaid { get; set; }
    public long? Paymentid { get; set; }

    // Follow-up Info
    public bool Isfollowup { get; set; }
    public long? Parentappointmentid { get; set; }

    // Audit
    public bool Isactive { get; set; }
    public DateTime Createddate { get; set; }
    public DateTime? Updateddate { get; set; }
}