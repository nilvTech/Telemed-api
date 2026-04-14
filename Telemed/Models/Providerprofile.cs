using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Providerprofile
{
    public long Profileid { get; set; }

    public long Providerinfoid { get; set; }

    public string Providertype { get; set; } = null!;

    public string? Bio { get; set; }

    public int? Yearofexperience { get; set; }

    public string? Licensenumber { get; set; }

    public string? NpiNumber { get; set; }

    public string? Secondaryrole { get; set; }

    public string Speciality1 { get; set; } = null!;

    public string? Speciality2 { get; set; }

    public string? Website { get; set; }

    public string Timezone { get; set; } = null!;

    public string Addressline1 { get; set; } = null!;

    public string? Addressline2 { get; set; }

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Zip { get; set; } = null!;

    public string? Country { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public long? Createdby { get; set; }

    public long? Updatedby { get; set; }

    public virtual ProviderInfo Providerinfo { get; set; } = null!;
}
