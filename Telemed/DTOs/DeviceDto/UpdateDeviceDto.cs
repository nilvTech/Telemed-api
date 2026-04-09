// DTOs/UpdateDeviceDto.cs
namespace Telemed.DTOs;

public class UpdateDeviceDto
{
    public string? Name { get; set; }
    public string? Manufacturer { get; set; }
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public bool? Status { get; set; }
    public IFormFile? DevicePicture { get; set; }
}