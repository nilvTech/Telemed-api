using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Message
{
    public int Messageid { get; set; }

    public long Patientid { get; set; }

    public long Providerid { get; set; }

    public string Sendertype { get; set; } = null!;

    public string Messagetext { get; set; } = null!;

    public DateTime? Sentat { get; set; }

    public bool? Isread { get; set; }

    public virtual Patient Patient { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;
}
