using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Videosession
{
    public int Videosessionid { get; set; }

    public long Encounterid { get; set; }

    public long Providerid { get; set; }

    public long Patientid { get; set; }

    public DateTime? Starttime { get; set; }

    public DateTime? Endtime { get; set; }

    public string? Callstatus { get; set; }

    public string? Recordingurl { get; set; }

    public string? Videoname { get; set; }

    public byte[]? Videodata { get; set; }

    public long? Videosize { get; set; }

    public int? Durationseconds { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Encounter Encounter { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;
}
