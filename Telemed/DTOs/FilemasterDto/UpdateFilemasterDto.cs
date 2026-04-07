// DTOs/UpdateFilemasterDto.cs
namespace Telemed.DTOs;

public class UpdateFilemasterDto
{
    public string? Filename { get; set; }
    public int? Uploadedchunks { get; set; }
    public bool? Iscompleted { get; set; }
    public long? Updatedby { get; set; }
}