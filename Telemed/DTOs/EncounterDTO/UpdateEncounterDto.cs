// DTOs/UpdateEncounterDto.cs
namespace Telemed.DTOs;

public class UpdateEncounterDto
{
    public DateTime? Encounterdate { get; set; }
    public string? Subjective { get; set; }
    public string? Objective { get; set; }
    public string? Assessment { get; set; }
    public string? Plan { get; set; }
    public string? Diagnosis { get; set; }
    public string? Icd10code { get; set; }
    public string? Notes { get; set; }
}