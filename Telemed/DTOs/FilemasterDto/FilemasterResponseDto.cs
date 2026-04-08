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
    public string? Filesizeformatted { get; set; }
    public int Totalchunks { get; set; }
    public int? Uploadedchunks { get; set; }
    public int? Uploadprogresspercent { get; set; }
    public bool? Iscompleted { get; set; }

    public int Storedchunkcount { get; set; } // ✅ ADD THIS

    // Audit
    public DateTime? Createddate { get; set; }
    public long? Createdby { get; set; }
    public long? Updatedby { get; set; }
    public DateTime? Updateddate { get; set; }
}