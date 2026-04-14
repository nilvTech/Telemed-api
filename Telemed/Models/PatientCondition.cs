using System.ComponentModel.DataAnnotations.Schema;
using Telemed.Models;

public class PatientCondition
{
    public long PatientConditionId { get; set; }
    public long PatientId { get; set; }
    public long ConditionId { get; set; }

    [Column("consultationid")]
    public long? ConsultationId { get; set; }

    public long? ProviderInfoId { get; set; }
    public string? Status { get; set; }
    public DateTime? OnsetDate { get; set; }
    public string? Note { get; set; }
    public string? ManagedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public long? UpdatedBy { get; set; }

    // ✅ Navigation properties
    public virtual Patient Patient { get; set; } = null!;
    public virtual ConditionMaster ConditionMaster { get; set; } = null!;
    public virtual ProviderInfo? ProviderInfo { get; set; }   // <-- must exist

    // public virtual Consultation? Consultation { get; set; }

    // Navigation property back to parent
    [ForeignKey("ConsultationId")]
    public virtual Consultation? Consultation { get; set; }
}
