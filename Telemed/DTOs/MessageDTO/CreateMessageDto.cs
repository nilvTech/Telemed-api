// DTOs/CreateMessageDto.cs
namespace Telemed.DTOs;

public class CreateMessageDto
{
    public long Patientid { get; set; }
    public long Providerid { get; set; }
    public string Sendertype { get; set; } = null!;  // "Patient" or "Provider"
    public string Messagetext { get; set; } = null!;
}