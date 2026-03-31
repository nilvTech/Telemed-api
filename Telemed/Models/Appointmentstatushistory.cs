using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Appointmentstatushistory
{
    public long Id { get; set; }

    public long? Appointmentid { get; set; }

    public string? Status { get; set; }

    public DateTime? Changedat { get; set; }

    public long? Changedby { get; set; }
}
