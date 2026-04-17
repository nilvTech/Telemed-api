// DTOs/CareplanStatusUpdateDto.cs
namespace Telemed.DTOs;

public class CareplanStatusUpdateDto
{
    public string Status { get; set; } = null!;
    public long? Updatedby { get; set; }
}