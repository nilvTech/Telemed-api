using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Filemaster
{
    public long Fileid { get; set; }

    public long? Patientid { get; set; }

    public string Filename { get; set; } = null!;

    public string Filetype { get; set; } = null!;

    public long Totalsize { get; set; }

    public int Totalchunks { get; set; }

    public int? Uploadedchunks { get; set; }

    public bool? Iscompleted { get; set; }

    public DateTime? Createddate { get; set; }

    public long? Createdby { get; set; }

    public long? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public List<byte[]>? Pdfcontent { get; set; }

    // Clinical Order
    public long? Clinicalorderid { get; set; }

    public virtual Clinicalorder? Clinicalorder { get; set; }


}


