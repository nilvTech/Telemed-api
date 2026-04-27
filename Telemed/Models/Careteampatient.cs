using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Careteampatient
{
    public long Careteampatientid { get; set; }

    public long Patientid { get; set; }

    public long Careteamid { get; set; }

    public DateTime? Assigneddate { get; set; }

    public bool? Isactive { get; set; }

    public virtual Careteam Careteam { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
