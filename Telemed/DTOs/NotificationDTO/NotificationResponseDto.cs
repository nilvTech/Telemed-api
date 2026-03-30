// DTOs/NotificationResponseDto.cs
namespace Telemed.DTOs;

public class NotificationResponseDto
{
    public int Notificationid { get; set; }
    public string Usertype { get; set; } = null!;
    public int Userid { get; set; }
    public string? Username { get; set; }   // Resolved Patient or Provider name
    public string? Title { get; set; }
    public string? Message { get; set; }
    public bool? Isread { get; set; }
    public DateTime? Createdat { get; set; }
}