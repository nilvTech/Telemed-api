// DTOs/CreateClaimDto.cs
namespace Telemed.DTOs;

public class CreateClaimDto
{
    public long Patientid { get; set; }
    public long Providerinfoid { get; set; }
    public string? Payer { get; set; }
    public string? Claimnumber { get; set; }
    public int? Totaltime { get; set; }
    public string? Icdcode { get; set; }
    public DateOnly? Submissiondate { get; set; }
    public long? Createdby { get; set; }

    // Claim details submitted together
    public List<CreateClaimDetailDto> Claimdetails { get; set; }
        = new List<CreateClaimDetailDto>();
}