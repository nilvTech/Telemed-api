// DTOs/AdminclaimStatusUpdateDto.cs
namespace Telemed.DTOs;

public class AdminclaimStatusUpdateDto
{
    public string Status { get; set; } = null!;
    public string? Lastaction { get; set; }
}