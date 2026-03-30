using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Appointment
{
    public int Appointmentid { get; set; }

    public int Patientid { get; set; }

    public int Providerid { get; set; }

    public DateTime Scheduleddatetime { get; set; }

    public string? Status { get; set; }

    public string? Mode { get; set; }

    public string? Notes { get; set; }

    public DateTime? Createdate { get; set; }

    public DateTime? Updatedate { get; set; }

    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Provider Provider { get; set; } = null!;
}
