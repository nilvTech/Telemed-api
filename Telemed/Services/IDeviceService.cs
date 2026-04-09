// Services/Interfaces/IDeviceService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IDeviceService
{
    Task<DeviceResponseDto> CreateAsync(CreateDeviceDto dto);
    Task<IEnumerable<DeviceResponseDto>> GetAllAsync();
    Task<DeviceResponseDto?> GetByIdAsync(long id);
    Task<DeviceResponseDto?> GetByDeviceIdAsync(string deviceId);
    Task<DeviceResponseDto?> GetBySerialNumberAsync(string serialNumber);
    Task<IEnumerable<DeviceResponseDto>> GetByManufacturerAsync(string manufacturer);
    Task<IEnumerable<DeviceResponseDto>> GetActiveAsync();
    Task<DeviceResponseDto?> UpdateAsync(long id, UpdateDeviceDto dto);
    Task<DeviceResponseDto?> UpdatePictureAsync(long id, IFormFile picture);
    Task<byte[]?> GetDevicePictureAsync(long id);
    Task<bool> ToggleStatusAsync(long id);
    Task<bool> DeleteAsync(long id);
}