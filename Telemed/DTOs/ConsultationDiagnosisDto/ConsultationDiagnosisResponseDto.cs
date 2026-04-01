// DTOs/ConsultationDiagnosisResponseDto.cs
namespace Telemed.DTOs;

public class ConsultationDiagnosisResponseDto
{
    public long Id { get; set; }
    public long Consultationid { get; set; }

    // Consultation Info
    public string? Consultationstatus { get; set; }

    // Patient Info
    public long? Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long? Providerid { get; set; }
    public string? Providername { get; set; }

    // Diagnosis Details
    public string? Diagnosiscode { get; set; }
    public string? Diagnosisname { get; set; }
}