using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Vital
{
    public int Vitalsid { get; set; }

    public int Encounterid { get; set; }

    public int Patientid { get; set; }

    public int? Heartrate { get; set; }

    public string? Bloodpressure { get; set; }

    public int? Respiratoryrate { get; set; }

    public decimal? Temperature { get; set; }

    public int? Oxygensaturation { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Bmi { get; set; }

    public DateTime? Recordedat { get; set; }

    public virtual Encounter Encounter { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
