using System;
using System.Collections.Generic;

namespace Telemed.Models;   
public partial class ProviderGroup
{
    public long GroupId { get; set; }                    
    public string GroupName { get; set; } = null!;       
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Speciality { get; set; }
    public string? Website { get; set; }
    public string? Bio { get; set; }
    public string AddressLine1 { get; set; } = null!;    
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string Zip { get; set; } = null!;
    public string? Country { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }

    // Navigation Property - Many-to-Many
    public virtual ICollection<ProviderGroup_Member> GroupMembers { get; set; }
        = new List<ProviderGroup_Member>();
}