// DTOs/CheckInDto.cs
namespace Telemed.DTOs;

public class CheckInDto
{
    public int? Tokennumber { get; set; }
    public int? Queueposition { get; set; }
    public long? Updatedby { get; set; }
}