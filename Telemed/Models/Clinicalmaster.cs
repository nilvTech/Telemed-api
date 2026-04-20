using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Clinicalmaster
{
    public long Clinicalmasterid { get; set; }

    public string Ordertype { get; set; } = null!;

    public string Ordername { get; set; } = null!;

    public string Ordercode { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual ICollection<Clinicalorder> Clinicalorders { get; set; } = new List<Clinicalorder>();

}
