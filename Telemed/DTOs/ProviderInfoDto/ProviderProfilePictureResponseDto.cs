// DTOs/ProviderProfilePictureResponseDto.cs
namespace Telemed.DTOs;

public class ProviderProfilePictureResponseDto
{
    public long Providerinfoid { get; set; }
    public string? Fullname { get; set; }
    public string? ProfilepictureBase64 { get; set; }
    public bool HasProfilePicture { get; set; }
}