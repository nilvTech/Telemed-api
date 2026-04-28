using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Claim
{
    public long Claimid { get; set; }

    public long Patientid { get; set; }

    public long Providerinfoid { get; set; }

    public string? Payer { get; set; }

    public string? Claimnumber { get; set; }

    public int? Totaltime { get; set; }

    public decimal Totalamount { get; set; }

    public string? Status { get; set; }

    public DateOnly? Submissiondate { get; set; }

    public decimal? Paidamount { get; set; }

    public string? Deniedreason { get; set; }

    public string? Icdcode { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public long? Createdby { get; set; }

    public long? Updatedby { get; set; }

    // NAVIGATION PROPERTIES
    public virtual Patient? Patient { get; set; }
    public virtual ProviderInfo? Providerinfo { get; set; }

    public virtual ICollection<Claimdetail> Claimdetails { get; set; } = new List<Claimdetail>();

    public virtual ICollection<Adminclaim> Adminclaims { get; set; } = new List<Adminclaim>();

}
