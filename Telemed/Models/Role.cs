using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Role
{
    public long Roleid { get; set; }

    public string Rolename { get; set; } = null!;

    public string Rolecode { get; set; } = null!;

    public string Roletype { get; set; } = null!;

    public string Accesslevel { get; set; } = null!;

    public string? Status { get; set; }

    public string? Defaultlandingpage { get; set; }

    public string? Datascope { get; set; }

    public bool? Requiresmfa { get; set; }

    public string? Description { get; set; }

    public long? Createdby { get; set; }

    public DateTime? Createdat { get; set; }

    public long? Updatedby { get; set; }

    public DateTime? Updatedat { get; set; }
}
