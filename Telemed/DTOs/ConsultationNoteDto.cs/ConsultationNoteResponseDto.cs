// DTOs/ConsultationNoteResponseDto.cs
namespace Telemed.DTOs;

public class ConsultationNoteResponseDto
{
    public long Id { get; set; }
    public long Consultationid { get; set; }

    // Consultation Info
    public string? Consultationstatus { get; set; }
    public DateTime? Consultationstarttime { get; set; }

    // Patient Info
    public long? Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long? Providerid { get; set; }
    public string? Providername { get; set; }
    public string? Speciality { get; set; }

    // Note Details
    public string? Notes { get; set; }
    public DateTime? Createdat { get; set; }
}