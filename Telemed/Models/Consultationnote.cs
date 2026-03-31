using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Consultationnote
{
    public long Id { get; set; }

    public long Consultationid { get; set; }

    public string? Notes { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual Consultation Consultation { get; set; } = null!;
}
