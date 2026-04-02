using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Patientfollowup
{
    public long Id { get; set; }

    public long Patientid { get; set; }

    public DateTime Followupdate { get; set; }

    public string? Notes { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
