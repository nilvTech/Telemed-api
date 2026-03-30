// DTOs/CreateNotificationDto.cs
namespace Telemed.DTOs;

public class CreateNotificationDto
{
    public string Usertype { get; set; } = null!;  // "Patient" or "Provider"
    public int Userid { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
}