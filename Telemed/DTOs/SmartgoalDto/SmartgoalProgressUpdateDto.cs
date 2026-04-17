// DTOs/SmartgoalProgressUpdateDto.cs
namespace Telemed.DTOs;

public class SmartgoalProgressUpdateDto
{
    public decimal Currentvalue { get; set; }
    public string? Notes { get; set; }
    public long? Updatedby { get; set; }
}