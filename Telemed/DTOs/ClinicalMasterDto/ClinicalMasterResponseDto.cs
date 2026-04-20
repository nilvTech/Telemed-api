// DTOs/ClinicalMasterResponseDto.cs
namespace Telemed.DTOs;

public class ClinicalMasterResponseDto
{
    public long Clinicalmasterid { get; set; }
    public string? Ordertype { get; set; }
    public string? Ordername { get; set; }
    public string? Ordercode { get; set; }
    public string? Description { get; set; }
    public int TotalOrders { get; set; }
    public DateTime? Createdat { get; set; }
}