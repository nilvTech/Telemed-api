// DTOs/FileUploadResponseDto.cs
namespace Telemed.DTOs;

public class FileUploadResponseDto
{
    public long Fileid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }
    public string? Filename { get; set; }
    public string? Filetype { get; set; }
    public long Totalsize { get; set; }
    public string? Filesizeformatted { get; set; }
    public int Totalchunks { get; set; }
    public int Uploadedchunks { get; set; }
    public int Uploadprogresspercent { get; set; }
    public bool Iscompleted { get; set; }
    public string? Savedpath { get; set; }
    public DateTime? Createddate { get; set; }
}