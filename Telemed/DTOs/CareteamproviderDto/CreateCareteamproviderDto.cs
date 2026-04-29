// DTOs/CreateCareteamproviderDto.cs
namespace Telemed.DTOs;

public class CreateCareteamproviderDto
{
    public long Careteamid { get; set; }
    public long Providerinfoid { get; set; }
    public string? Role { get; set; }
}