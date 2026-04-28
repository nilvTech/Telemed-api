using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Adminclaim
{
    public long Adminclaimid { get; set; }

    public long Claimid { get; set; }

    public long Patientid { get; set; }

    public long Providerinfoid { get; set; }

    public long? Appointmentid { get; set; }

    public int? Encounterid { get; set; }

    public DateTime? Claimdate { get; set; }

    public string? Status { get; set; }

    public string? Lastaction { get; set; }

    public DateTime? Lastactiondate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Claim Claim { get; set; } = null!;

    public virtual Encounter? Encounter { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual ProviderInfo Providerinfo { get; set; } = null!;
}
