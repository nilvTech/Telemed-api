using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Appointmentdocument
{
    public long Id { get; set; }

    public long? Appointmentid { get; set; }

    public string? Fileurl { get; set; }

    public string? Filetype { get; set; }

    public DateTime? Uploadedat { get; set; }

    // ✅ Add navigation property
    public virtual Appointment? Appointment { get; set; }
}
