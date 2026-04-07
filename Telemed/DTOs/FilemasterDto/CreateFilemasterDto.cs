// DTOs/CreateFilemasterDto.cs
namespace Telemed.DTOs;

public class CreateFilemasterDto
{
    public long? Patientid { get; set; }
    public string Filename { get; set; } = null!;
    public string Filetype { get; set; } = null!;
    public long Totalsize { get; set; }
    public int Totalchunks { get; set; }
    public long? Createdby { get; set; }
}