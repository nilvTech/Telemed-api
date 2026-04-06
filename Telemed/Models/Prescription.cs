using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Prescription
{
    public int Prescriptionid { get; set; }

    public long Encounterid { get; set; }

    public long Patientid { get; set; }

    public long Providerid { get; set; }

    public string Medicinename { get; set; } = null!;

    public string? Dosage { get; set; }

    public string? Frequency { get; set; }

    public string? Duration { get; set; }

    public string? Notes { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Encounter Encounter { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;
}
