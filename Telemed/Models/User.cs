using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public long Referenceid { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Lastloginat { get; set; }

    public string? Refreshtoken { get; set; }

    public DateTime? Refreshtokenexpiry { get; set; }
}
