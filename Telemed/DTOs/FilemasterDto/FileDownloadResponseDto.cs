// DTOs/FileDownloadResponseDto.cs
namespace Telemed.DTOs;

public class FileDownloadResponseDto
{
    public long Fileid { get; set; }
    public string? Filename { get; set; }
    public string? Filetype { get; set; }
    public byte[]? Filedata { get; set; }  // Merged chunks as single byte array
    public long Totalsize { get; set; }
    public string? Filesizeformatted { get; set; }
}