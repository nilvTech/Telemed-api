// DTOs/CareteamResponseDto.cs
namespace Telemed.DTOs;

public class CareteamResponseDto
{
    public long Careteamid { get; set; }
    public string? Teamname { get; set; }
    public string? Description { get; set; }
    public DateTime? Createdat { get; set; }
}