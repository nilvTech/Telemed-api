using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Consultationdiagnosis
{
    public long Id { get; set; }

    public long Consultationid { get; set; }

    public string? Diagnosiscode { get; set; }

    public string? Diagnosisname { get; set; }

    public virtual Consultation Consultation { get; set; } = null!;
}
