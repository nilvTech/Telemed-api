using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Followup
{
    public long Followupid { get; set; }

    public long Patientid { get; set; }

    public long? Appointmentid { get; set; }

    public DateOnly Followupdate { get; set; }

    public string Followuptype { get; set; } = null!;

    public string? Notes { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Appointment? Appointment { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
