using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Payment
{
    public int Paymentid { get; set; }

    public int Appointmentid { get; set; }

    public int Patientid { get; set; }

    public int Providerid { get; set; }

    public decimal Amount { get; set; }

    public string? Paymentmethod { get; set; }

    public DateTime? Paymentdate { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;
}
