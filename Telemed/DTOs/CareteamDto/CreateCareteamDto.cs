// DTOs/CreateCareteamDto.cs
namespace Telemed.DTOs;

public class CreateCareteamDto
{
    public string Teamname { get; set; } = null!;
    public string? Description { get; set; }
}