using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Patientalert
{
    public long Alertid { get; set; }

    public long Patientid { get; set; }

    public string Alerttype { get; set; } = null!;

    public string Alertmessage { get; set; } = null!;

    public bool Isread { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime? Updateddate { get; set; }

    public bool Isactive { get; set; }

    public string? Severity { get; set; }

    public bool? Isacknowledged { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
