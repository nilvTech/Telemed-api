using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class PatientTask
{
    public long Taskid { get; set; }

    public string Taskname { get; set; } = null!;

    public DateOnly Duedate { get; set; }

    public long Patientid { get; set; }

    public long? Providerinfoid { get; set; }

    public string? Status { get; set; }

    public string? Priority { get; set; }

    public string? Description { get; set; }

    public DateOnly? Completeddate { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public long? Createdby { get; set; }

    public long? Updatedby { get; set; }

    // ADD NAVIGATION PROPERTIES
    public virtual Patient Patient { get; set; } = null!;
    public virtual ProviderInfo? Providerinfo { get; set; }
}
