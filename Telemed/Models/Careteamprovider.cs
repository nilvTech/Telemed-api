using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Careteamprovider
{
    public long Careteamproviderid { get; set; }

    public long Careteamid { get; set; }

    public long Providerinfoid { get; set; }

    public string? Role { get; set; }

    public DateTime? Assigneddate { get; set; }

    public bool? Isactive { get; set; }

    public virtual Careteam Careteam { get; set; } = null!;

    public virtual ProviderInfo Providerinfo { get; set; } = null!;
}
