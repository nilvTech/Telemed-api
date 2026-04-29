// DTOs/CreateClaimformDto.cs
namespace Telemed.DTOs;

public class CreateClaimformDto
{
    // Patient Information
    public string? Patientname { get; set; }
    public DateOnly? Dateofbirth { get; set; }
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

    public long? Createdby { get; set; }
}