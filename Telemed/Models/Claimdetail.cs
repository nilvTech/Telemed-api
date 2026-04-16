using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Claimdetail
{
    public long Claimdetailid { get; set; }

    public long Claimid { get; set; }

    public string Cptcode { get; set; } = null!;

    public string? Description { get; set; }

    public int? Units { get; set; }

    public decimal Amount { get; set; }

    public virtual Claim? Claim { get; set; } = null!;
}
