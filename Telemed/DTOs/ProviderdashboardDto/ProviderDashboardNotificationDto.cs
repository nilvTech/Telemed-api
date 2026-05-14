// DTOs/ProviderDashboardNotificationDto.cs
namespace Telemed.DTOs;

public class ProviderDashboardNotificationDto
{
    public long Notificationid { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public string? Severity { get; set; }    // High / Medium / Low
    public string? Timeago { get; set; }     // "2 hours ago"
    public DateTime? Createdat { get; set; }
    public bool? Isread { get; set; }
}