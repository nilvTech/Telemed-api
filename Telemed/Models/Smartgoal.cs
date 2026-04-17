using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Smartgoal
{
    public long Smartgoalid { get; set; }

    public long Careplanid { get; set; }

    public long Patientid { get; set; }

    public long? Providerinfoid { get; set; }

    public string Goaltitle { get; set; } = null!;

    public string? Description { get; set; }

    public string Measurementtype { get; set; } = null!;

    public decimal? Targetvalue { get; set; }

    public decimal? Currentvalue { get; set; }

    public string? Unit { get; set; }

    public DateOnly Startdate { get; set; }

    public DateOnly? Targetdate { get; set; }

    public string? Status { get; set; }

    public decimal? Progress { get; set; }

    public string? Diettype { get; set; }

    public string? Exercisetype { get; set; }

    public int? Weeklyminutes { get; set; }

    public string? Notes { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public long? Createdby { get; set; }

    public long? Updatedby { get; set; }

    public virtual Careplan Careplan { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual ProviderInfo? Providerinfo { get; set; }
}
