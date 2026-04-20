using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Clinicalorder
{
    public long Clinicalorderid { get; set; }

    public long Patientid { get; set; }

    public int? Encounterid { get; set; }

    public long Providerinfoid { get; set; }

    public long Clinicalmasterid { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public DateTime? Orderdate { get; set; }

    public DateTime? Completeddate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Clinicalmaster Clinicalmaster { get; set; } = null!;

    public virtual Encounter? Encounter { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual ProviderInfo Providerinfo { get; set; } = null!;



}
