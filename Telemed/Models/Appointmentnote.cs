using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Appointmentnote
{
    public long Id { get; set; }

    public long? Appointmentid { get; set; }

    public string? Notes { get; set; }

    public DateTime? Createdat { get; set; }
}
