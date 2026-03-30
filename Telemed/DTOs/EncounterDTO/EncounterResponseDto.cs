// DTOs/EncounterResponseDto.cs
namespace Telemed.DTOs;

public class EncounterResponseDto
{
    public int Encounterid { get; set; }

    // Appointment Info
    public int Appointmentid { get; set; }
    public DateTime? Appointmentdate { get; set; }

    // Patient Info
    public int Patientid { get; set; }
    public string? Patientname { get; set; }

    // Provider Info
    public int Providerid { get; set; }
    public string? Providername { get; set; }
    public string? Speciality { get; set; }

    public DateTime Encounterdate { get; set; }
    public string? Subjective { get; set; }
    public string? Objective { get; set; }
    public string? Assessment { get; set; }
    public string? Plan { get; set; }
    public string? Diagnosis { get; set; }
    public string? Icd10code { get; set; }
    public string? Notes { get; set; }
    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}