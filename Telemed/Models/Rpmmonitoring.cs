using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Rpmmonitoring
{
    public long Rpmmonitoringid { get; set; }

    public long Patientid { get; set; }

    public DateTime Readingdate { get; set; }

    public int? Systolic { get; set; }

    public int? Diastolic { get; set; }

    public int? Heartrate { get; set; }

    public int? Spo2 { get; set; }

    public int? Glucose { get; set; }

    public string? Glucoseunit { get; set; }

    public decimal? Temperature { get; set; }

    public string? Temperatureunit { get; set; }

    public decimal? Weight { get; set; }

    public string? Weightunit { get; set; }

    public decimal? Height { get; set; }

    public string? Heightunit { get; set; }

    public int? Respiratoryrate { get; set; }

    public string? Devicetype { get; set; }

    public string? Deviceid { get; set; }

    public string? Sourcedata { get; set; }

    public bool? Isauto { get; set; }

    public bool? Isreviewed { get; set; }

    public DateTime? Reviewedat { get; set; }

    public long? Reviewedby { get; set; }

    public string? Note { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public long? Createdby { get; set; }

    public long? Updatedby { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual ProviderInfo? ReviewedByProvider { get; set; }


}
