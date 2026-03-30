// DTOs/CreateEncounterDto.cs
namespace Telemed.DTOs;

public class CreateEncounterDto
{
    public int Appointmentid { get; set; }
    public int Patientid { get; set; }
    public int Providerid { get; set; }
    public DateTime Encounterdate { get; set; }
    public string? Subjective { get; set; }
    public string? Objective { get; set; }
    public string? Assessment { get; set; }
    public string? Plan { get; set; }
    public string? Diagnosis { get; set; }
    public string? Icd10code { get; set; }
    public string? Notes { get; set; }
}