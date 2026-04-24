using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Careteam
{
    public long Careteamid { get; set; }

    public string Teamname { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? Createdat { get; set; }
}
