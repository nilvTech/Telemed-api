using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Admin
{
    public int Adminid { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime? Createdate { get; set; }
}
