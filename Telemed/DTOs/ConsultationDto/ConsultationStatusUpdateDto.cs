// DTOs/ConsultationStatusUpdateDto.cs
namespace Telemed.DTOs;

public class ConsultationStatusUpdateDto
{
    public string Status { get; set; } = null!;
    public long? Updatedby { get; set; }
}