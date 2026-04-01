// DTOs/ConsultationResponseDto.cs
namespace Telemed.DTOs;

public class ConsultationResponseDto
{
    public long Consultationid { get; set; }

    // Appointment Info
    public long Appointmentid { get; set; }
    public DateOnly? Appointmentdate { get; set; }
    public string? Visittype { get; set; }
    public string? Visitmode { get; set; }

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

    // Consultation Details
    public DateTime? Starttime { get; set; }
    public DateTime? Endtime { get; set; }
    public int? Durationminutes { get; set; }
    public string? Chiefcomplaint { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatmentplan { get; set; }
    public string? Notes { get; set; }

    // Vitals
    public decimal? Temperature { get; set; }
    public string? Bloodpressure { get; set; }
    public int? Pulse { get; set; }
    public int? Respiratoryrate { get; set; }
    public int? Oxygensaturation { get; set; }

    public string? Status { get; set; }

    // Follow-up
    public DateOnly? Followupdate { get; set; }
    public string? Followupnotes { get; set; }

    public string? Recordingurl { get; set; }
    public bool Isprescriptiongenerated { get; set; }
    public bool Isactive { get; set; }
    public DateTime Createddate { get; set; }
    public DateTime? Updateddate { get; set; }
}