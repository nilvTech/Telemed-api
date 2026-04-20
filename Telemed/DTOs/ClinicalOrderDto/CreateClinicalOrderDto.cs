// DTOs/CreateClinicalOrderDto.cs
namespace Telemed.DTOs;

public class CreateClinicalOrderDto
{
    public long Patientid { get; set; }
    public int? Encounterid { get; set; }
    public long Providerinfoid { get; set; }
    public long Clinicalmasterid { get; set; }
    public string? Priority { get; set; }
}