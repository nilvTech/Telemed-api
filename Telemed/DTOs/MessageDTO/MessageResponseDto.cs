// DTOs/MessageResponseDto.cs
namespace Telemed.DTOs;

public class MessageResponseDto
{
    public int Messageid { get; set; }

    // Patient Info
    public int Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Patientemail { get; set; }
    public string? Patientphone { get; set; }

    // Provider Info
    public int Providerid { get; set; }
    public string? Providername { get; set; }
    public string? Speciality { get; set; }

    // Message Details
    public string Sendertype { get; set; } = null!;
    public string? Sendername { get; set; }
    public string Messagetext { get; set; } = null!;
    public DateTime? Sentat { get; set; }
    public bool? Isread { get; set; }
}