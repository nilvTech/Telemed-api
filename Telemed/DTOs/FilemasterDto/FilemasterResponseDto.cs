// DTOs/FilemasterResponseDto.cs
namespace Telemed.DTOs;

public class FilemasterResponseDto
{
    public long Fileid { get; set; }

    // Patient Info
    public long? Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // File Details
    public string? Filename { get; set; }
    public string? Filetype { get; set; }
    public long Totalsize { get; set; }
    public string? Filesizeformatted { get; set; }  // e.g. "2.5 MB"
    public int Totalchunks { get; set; }
    public int? Uploadedchunks { get; set; }
    public int? Uploadprogresspercent { get; set; } // Auto calculated
    public bool? Iscompleted { get; set; }

    // Audit
    public DateTime? Createddate { get; set; }
    public long? Createdby { get; set; }
    public long? Updatedby { get; set; }
    public DateTime? Updateddate { get; set; }
}