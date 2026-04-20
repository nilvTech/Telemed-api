// DTOs/CreateClinicalMasterDto.cs
namespace Telemed.DTOs;

public class CreateClinicalMasterDto
{
    public string Ordertype { get; set; } = null!;
    public string Ordername { get; set; } = null!;
    public string Ordercode { get; set; } = null!;
    public string? Description { get; set; }
}