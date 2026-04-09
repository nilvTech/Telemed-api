// Services/DeviceService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class DeviceService : IDeviceService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidImageTypes = new[]
    {
        "image/jpeg", "image/jpg",
        "image/png", "image/gif", "image/webp"
    };

    private const long MaxPictureSize = 5 * 1024 * 1024; // 5MB

    public DeviceService(TelemedDbContext context)
    {
        _context = context;
    }

    private async Task<byte[]?> ReadPictureAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0) return null;

        if (!ValidImageTypes.Contains(file.ContentType.ToLower()))
            throw new ArgumentException(
                "Invalid image type. Allowed: JPEG, PNG, GIF, WebP.");

        if (file.Length > MaxPictureSize)
            throw new ArgumentException(
                "Device picture cannot exceed 5MB.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<DeviceResponseDto> CreateAsync(CreateDeviceDto dto)
    {
        // Validate SerialNumber unique
        var serialExists = await _context.Devices
            .AnyAsync(d => d.SerialNumber == dto.SerialNumber);
        if (serialExists)
            throw new ArgumentException(
                $"Device with serial number '{dto.SerialNumber}' already exists.");

        // Validate DeviceId unique
        var deviceIdExists = await _context.Devices
            .AnyAsync(d => d.DeviceId == dto.DeviceId);
        if (deviceIdExists)
            throw new ArgumentException(
                $"Device with device ID '{dto.DeviceId}' already exists.");

        var pictureBytes = await ReadPictureAsync(dto.DevicePicture);
        var entity = DeviceMapper.ToEntity(dto, pictureBytes);

        _context.Devices.Add(entity);
        await _context.SaveChangesAsync();

        return DeviceMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<DeviceResponseDto>> GetAllAsync()
    {
        var list = await _context.Devices
            .OrderBy(d => d.Name)
            .ToListAsync();

        return list.Select(DeviceMapper.ToResponseDto);
    }

    public async Task<DeviceResponseDto?> GetByIdAsync(long id)
    {
        var entity = await _context.Devices
            .FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null) return null;
        return DeviceMapper.ToResponseDto(entity);
    }

    public async Task<DeviceResponseDto?> GetByDeviceIdAsync(string deviceId)
    {
        var entity = await _context.Devices
            .FirstOrDefaultAsync(d => d.DeviceId == deviceId);

        if (entity == null) return null;
        return DeviceMapper.ToResponseDto(entity);
    }

    public async Task<DeviceResponseDto?> GetBySerialNumberAsync(
        string serialNumber)
    {
        var entity = await _context.Devices
            .FirstOrDefaultAsync(d => d.SerialNumber == serialNumber);

        if (entity == null) return null;
        return DeviceMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<DeviceResponseDto>> GetByManufacturerAsync(
        string manufacturer)
    {
        var list = await _context.Devices
            .Where(d => d.Manufacturer.ToLower()
                         .Contains(manufacturer.ToLower()))
            .OrderBy(d => d.Name)
            .ToListAsync();

        return list.Select(DeviceMapper.ToResponseDto);
    }

    public async Task<IEnumerable<DeviceResponseDto>> GetActiveAsync()
    {
        var list = await _context.Devices
            .Where(d => d.Status == true)
            .OrderBy(d => d.Name)
            .ToListAsync();

        return list.Select(DeviceMapper.ToResponseDto);
    }

    public async Task<DeviceResponseDto?> UpdateAsync(
        long id, UpdateDeviceDto dto)
    {
        var entity = await _context.Devices
            .FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null) return null;

        var pictureBytes = await ReadPictureAsync(dto.DevicePicture);
        DeviceMapper.UpdateEntity(entity, dto, pictureBytes);
        await _context.SaveChangesAsync();

        return DeviceMapper.ToResponseDto(entity);
    }

    public async Task<DeviceResponseDto?> UpdatePictureAsync(
        long id, IFormFile picture)
    {
        var entity = await _context.Devices
            .FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null) return null;

        var pictureBytes = await ReadPictureAsync(picture);
        if (pictureBytes == null)
            throw new ArgumentException("Invalid picture file.");

        entity.DevicePicture = pictureBytes;
        entity.UpdatedAt = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return DeviceMapper.ToResponseDto(entity);
    }

    public async Task<byte[]?> GetDevicePictureAsync(long id)
    {
        var entity = await _context.Devices
            .FirstOrDefaultAsync(d => d.Id == id);

        return entity?.DevicePicture;
    }

    public async Task<bool> ToggleStatusAsync(long id)
    {
        var entity = await _context.Devices
            .FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null) return false;

        entity.Status = !(entity.Status ?? true);
        entity.UpdatedAt = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Devices.FindAsync(id);
        if (entity == null) return false;

        _context.Devices.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}