using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class ProviderInfo
{
    public long Providerinfoid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string Role { get; set; } = null!;

    public string? Displayname { get; set; }

    public byte[]? Profilepicture { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public long? Createdby { get; set; }

    public long? Updatedby { get; set; }

    public virtual Providerprofile? Providerprofile { get; set; }

    public string? GroupName { get; set; }


    // Add this 
    public virtual ICollection<ProviderGroup_Member> GroupMemberships { get; set; }
    = new List<ProviderGroup_Member>();

    // Provider Condition

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();



}
