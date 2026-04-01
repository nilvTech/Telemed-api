// DTOs/CreateConsultationNoteDto.cs
namespace Telemed.DTOs;

public class CreateConsultationNoteDto
{
    public long Consultationid { get; set; }
    public string Notes { get; set; } = null!;
}