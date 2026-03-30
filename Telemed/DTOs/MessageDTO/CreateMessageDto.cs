// DTOs/CreateMessageDto.cs
namespace Telemed.DTOs;

public class CreateMessageDto
{
    public int Patientid { get; set; }
    public int Providerid { get; set; }
    public string Sendertype { get; set; } = null!;  // "Patient" or "Provider"
    public string Messagetext { get; set; } = null!;
}