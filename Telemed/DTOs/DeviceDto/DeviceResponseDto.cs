// DTOs/DeviceResponseDto.cs
namespace Telemed.DTOs;

public class DeviceResponseDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? SerialNumber { get; set; }
    public string? DeviceId { get; set; }
    public string? Manufacturer { get; set; }
    public string? ModelNumber { get; set; }
    public string? Description { get; set; }
    public bool? Status { get; set; }
    public string? StatusLabel { get; set; }       // "Active" / "Inactive"
    public bool HasDevicePicture { get; set; }
    public string? DevicePictureBase64 { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}