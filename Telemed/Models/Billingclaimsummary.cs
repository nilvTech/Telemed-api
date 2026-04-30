using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Billingclaimsummary
{
    public long Summaryid { get; set; }

    public long Claimid { get; set; }

    public long Patientid { get; set; }

    public long Providerinfoid { get; set; }

    public string? Cptcode { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime Claimdate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Claim Claim { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual ProviderInfo Providerinfo { get; set; } = null!;
}
