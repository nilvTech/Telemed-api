// DTOs/CreateConsultationDiagnosisDto.cs
namespace Telemed.DTOs;

public class CreateConsultationDiagnosisDto
{
    public long Consultationid { get; set; }
    public string? Diagnosiscode { get; set; }   // ICD-10 code eg: I10
    public string? Diagnosisname { get; set; }   // eg: Essential Hypertension
}