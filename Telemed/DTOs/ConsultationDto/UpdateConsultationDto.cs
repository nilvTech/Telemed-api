// DTOs/UpdateConsultationDto.cs
namespace Telemed.DTOs;

public class UpdateConsultationDto
{
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
    public bool? Isprescriptiongenerated { get; set; }
    public bool? Isactive { get; set; }
    public long? Updatedby { get; set; }
}