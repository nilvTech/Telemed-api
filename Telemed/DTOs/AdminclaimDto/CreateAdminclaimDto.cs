// DTOs/CreateAdminclaimDto.cs
namespace Telemed.DTOs;

public class CreateAdminclaimDto
{
    public long Claimid { get; set; }
    public long Patientid { get; set; }
    public long Providerinfoid { get; set; }
    public long? Appointmentid { get; set; }
    public int? Encounterid { get; set; }
    public DateTime? Claimdate { get; set; }
    public string? Status { get; set; }
    public string? Lastaction { get; set; }
}