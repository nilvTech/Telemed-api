// DTOs/RealFileUploadDto.cs
namespace Telemed.DTOs;

public class RealFileUploadDto
{
    public long? Patientid { get; set; }
    public long? Createdby { get; set; }
    public IFormFile File { get; set; } = null!;
}