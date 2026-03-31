using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Consultationprescription
{
    public long Prescriptionid { get; set; }

    public long Consultationid { get; set; }

    public string? Medicationname { get; set; }

    public string? Dosage { get; set; }

    public string? Frequency { get; set; }

    public string? Duration { get; set; }

    public string? Instructions { get; set; }

    public virtual Consultation Consultation { get; set; } = null!;
}
