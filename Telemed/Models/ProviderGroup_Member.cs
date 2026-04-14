using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class ProviderGroup_Member
{
    public long GroupMemberId { get; set; }          // Changed
    public long GroupId { get; set; }                // Changed
    public long ProviderInfoId { get; set; }         // Changed from providerinfoid

    public DateTime? JoinDate { get; set; }          // Changed
    public string? RoleInGroup { get; set; }         // Changed
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public virtual ProviderGroup Group { get; set; } = null!;
    public virtual ProviderInfo ProviderInfo { get; set; } = null!;   // Note: Providerinfo (your existing class)

}