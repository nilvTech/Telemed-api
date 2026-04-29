// DTOs/ClaimformResponseDto.cs
namespace Telemed.DTOs;

public class ClaimformResponseDto
{
    public long Claimformid { get; set; }

    // Patient Information
    public string? Patientname { get; set; }
    public DateOnly? Dateofbirth { get; set; }
    public int? Age { get; set; }               // Auto calculated
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }

    // Insurance Details
    public string? Insuranceplan { get; set; }
    public string? Policynumber { get; set; }
    public string? Insuredname { get; set; }

    // Claim Details
    public DateOnly? Dateofillness { get; set; }
    public string? Referringprovider { get; set; }
    public string? Diagnosiscode { get; set; }

    // Service
    public DateOnly? Servicedate { get; set; }
    public string? Servicecptcode { get; set; }
    public string? Servicedescription { get; set; }
    public decimal? Serviceamount { get; set; }

    // Total
    public decimal? Totalamount { get; set; }

    public DateTime? Createdat { get; set; }
    public long? Createdby { get; set; }
}