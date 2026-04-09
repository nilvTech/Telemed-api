// Mappers/DeviceMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class DeviceMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Device ToEntity(
        CreateDeviceDto dto,
        byte[]? pictureBytes = null)
    {
        return new Device
        {
            Name = dto.Name,
            SerialNumber = dto.SerialNumber,
            DeviceId = dto.DeviceId,
            Manufacturer = dto.Manufacturer,
            ModelNumber = dto.ModelNumber,
            Description = dto.Description,
            Status = true,
            DevicePicture = pictureBytes,
            CreatedAt = ToUnspecified(DateTime.UtcNow),
            UpdatedAt = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Device entity,
        UpdateDeviceDto dto,
        byte[]? pictureBytes = null)
    {
        if (!string.IsNullOrEmpty(dto.Name))
            entity.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Manufacturer))
            entity.Manufacturer = dto.Manufacturer;

        if (!string.IsNullOrEmpty(dto.ModelNumber))
            entity.ModelNumber = dto.ModelNumber;

        if (!string.IsNullOrEmpty(dto.Description))
            entity.Description = dto.Description;

        if (dto.Status.HasValue)
            entity.Status = dto.Status;

        if (pictureBytes != null)
            entity.DevicePicture = pictureBytes;

        entity.UpdatedAt = ToUnspecified(DateTime.UtcNow);
    }

    public static DeviceResponseDto ToResponseDto(Device entity)
    {
        string? pictureBase64 = null;
        if (entity.DevicePicture != null && entity.DevicePicture.Length > 0)
            pictureBase64 = Convert.ToBase64String(entity.DevicePicture);

        return new DeviceResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            SerialNumber = entity.SerialNumber,
            DeviceId = entity.DeviceId,
            Manufacturer = entity.Manufacturer,
            ModelNumber = entity.ModelNumber,
            Description = entity.Description,
            Status = entity.Status,
            StatusLabel = entity.Status == true ? "Active" : "Inactive",
            HasDevicePicture = entity.DevicePicture != null &&
                                   entity.DevicePicture.Length > 0,
            DevicePictureBase64 = pictureBase64,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}