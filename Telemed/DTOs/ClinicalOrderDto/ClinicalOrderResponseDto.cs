// DTOs/ClinicalOrderResponseDto.cs
namespace Telemed.DTOs;

public class ClinicalOrderResponseDto
{
    public long Clinicalorderid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long Providerinfoid { get; set; }
    public string? Providername { get; set; }
    public string? Providerspeciality { get; set; }

    // Encounter Info
    public int? Encounterid { get; set; }
    public DateTime? Encounterdate { get; set; }

    // Order Master Info
    public long Clinicalmasterid { get; set; }
    public string? Ordertype { get; set; }
    public string? Ordername { get; set; }
    public string? Ordercode { get; set; }
    public string? Orderdescription { get; set; }

    // Order Details
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public bool IsUrgent { get; set; }
    public DateTime? Orderdate { get; set; }
    public DateTime? Completeddate { get; set; }

    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }


}