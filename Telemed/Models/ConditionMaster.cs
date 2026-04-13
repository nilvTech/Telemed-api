namespace Telemed.Models;

using Telemed.DTOs;

public class ConditionMaster
{
    public long ConditionId { get; set; }

    public string ConditionName { get; set; } = null!;

    public string IcdCode { get; set; } = null!;

    public string? Description { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();
}
