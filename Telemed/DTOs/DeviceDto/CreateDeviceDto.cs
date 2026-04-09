// DTOs/CreateDeviceDto.cs
namespace Telemed.DTOs;

public class CreateDeviceDto
{
    public string Name { get; set; } = null!;
    public string SerialNumber { get; set; } = null!;
    public string DeviceId { get; set; } = null!;
    public string Manufacturer { get; set; } = null!;
    public string ModelNumber { get; set; } = null!;
    public string? Description { get; set; }
    public IFormFile? DevicePicture { get; set; }
}