// DTOs/UploadChunkDto.cs
namespace Telemed.DTOs;

public class UploadChunkDto
{
    public long Fileid { get; set; }
    public int Chunknumber { get; set; }
    public long? Updatedby { get; set; }
    public IFormFile Chunkdata { get; set; } = null!;  // Actual chunk bytes
}