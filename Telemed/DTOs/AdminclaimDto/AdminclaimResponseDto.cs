// DTOs/AdminclaimResponseDto.cs
namespace Telemed.DTOs;

public class AdminclaimResponseDto
{
    public long Adminclaimid { get; set; }

    // Claim Info
    public long Claimid { get; set; }
    public string? Claimnumber { get; set; }
    public decimal? Claimtotalamount { get; set; }
    public string? Claimpayer { get; set; }
    public string? Claimstatus { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }

    // Provider Info
    public long Providerinfoid { get; set; }
    public string? Providername { get; set; }
    public string? Providerspeciality { get; set; }

    // Appointment Info
    public long? Appointmentid { get; set; }
    public DateOnly? Appointmentdate { get; set; }

    // Encounter Info
    public int? Encounterid { get; set; }
    public DateTime? Encounterdate { get; set; }

    // Admin Claim Details
    public DateTime? Claimdate { get; set; }
    public string? Status { get; set; }
    public string? Lastaction { get; set; }
    public DateTime? Lastactiondate { get; set; }

    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}