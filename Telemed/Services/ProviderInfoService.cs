// Services/ProviderInfoService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ProviderInfoService : IProviderInfoService
{
    private readonly TelemedDbContext _context;

    // Allowed image types for profile picture
    private static readonly string[] ValidImageTypes = new[]
    {
        "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp"
    };

    // Max profile picture size 5MB
    private const long MaxPictureSize = 5 * 1024 * 1024;

    public ProviderInfoService(TelemedDbContext context)
    {
        _context = context;
    }

    private async Task<byte[]?> ReadProfilePictureAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0) return null;

        if (!ValidImageTypes.Contains(file.ContentType.ToLower()))
            throw new ArgumentException(
                "Invalid image type. Allowed: JPEG, PNG, GIF, WebP.");

        if (file.Length > MaxPictureSize)
            throw new ArgumentException("Profile picture cannot exceed 5MB.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<ProviderInfoResponseDto> CreateAsync(CreateProviderInfoDto dto)
    {
        // Validate email unique
        var emailExists = await _context.Providerinfos
            .AnyAsync(p => p.Email == dto.Email);
        if (emailExists)
            throw new ArgumentException(
                $"Email '{dto.Email}' is already registered.");

        // Validate NPI unique if provided
        if (!string.IsNullOrEmpty(dto.NpiNumber))
        {
            var npiExists = await _context.Providerprofiles
                .AnyAsync(p => p.NpiNumber == dto.NpiNumber);
            if (npiExists)
                throw new ArgumentException(
                    $"NPI Number '{dto.NpiNumber}' already exists.");
        }

        // Validate years of experience
        if (dto.Yearofexperience.HasValue && dto.Yearofexperience < 0)
            throw new ArgumentException(
                "Years of experience cannot be negative.");

        // Read profile picture bytes
        var pictureBytes = await ReadProfilePictureAsync(dto.Profilepicture);

        // Create Providerinfo
        var providerinfo = ProviderInfoMapper.ToProviderinfoEntity(dto, pictureBytes);
        _context.Providerinfos.Add(providerinfo);
        await _context.SaveChangesAsync();

        // Create Providerprofile
        var profile = ProviderInfoMapper.ToProviderprofileEntity(
            dto, providerinfo.Providerinfoid);
        _context.Providerprofiles.Add(profile);
        await _context.SaveChangesAsync();

        // Reload with profile
        var created = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .FirstOrDefaultAsync(p => p.Providerinfoid == providerinfo.Providerinfoid);

        return ProviderInfoMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<ProviderInfoResponseDto>> GetAllAsync()
    {
        var list = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .OrderBy(p => p.Lastname)
            .ThenBy(p => p.Firstname)
            .ToListAsync();

        return list.Select(ProviderInfoMapper.ToResponseDto);
    }

    public async Task<ProviderInfoResponseDto?> GetByIdAsync(long id)
    {
        var entity = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .FirstOrDefaultAsync(p => p.Providerinfoid == id);

        if (entity == null) return null;
        return ProviderInfoMapper.ToResponseDto(entity);
    }

    public async Task<ProviderInfoResponseDto?> GetByEmailAsync(string email)
    {
        var entity = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .FirstOrDefaultAsync(p => p.Email.ToLower() == email.ToLower());

        if (entity == null) return null;
        return ProviderInfoMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<ProviderInfoResponseDto>> GetBySpecialityAsync(
        string speciality)
    {
        var list = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .Where(p => p.Providerprofile != null &&
                        (p.Providerprofile.Speciality1.ToLower()
                            .Contains(speciality.ToLower()) ||
                         (p.Providerprofile.Speciality2 != null &&
                          p.Providerprofile.Speciality2.ToLower()
                            .Contains(speciality.ToLower()))))
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(ProviderInfoMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ProviderInfoResponseDto>> GetByStateAsync(
        string state)
    {
        var list = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .Where(p => p.Providerprofile != null &&
                        p.Providerprofile.State.ToLower() == state.ToLower() &&
                        p.Providerprofile.Isactive == true)
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(ProviderInfoMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ProviderInfoResponseDto>> GetActiveAsync()
    {
        var list = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .Where(p => p.Providerprofile != null &&
                        p.Providerprofile.Isactive == true)
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(ProviderInfoMapper.ToResponseDto);
    }

    public async Task<ProviderInfoResponseDto?> UpdateAsync(
        long id, UpdateProviderInfoDto dto)
    {
        var entity = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .FirstOrDefaultAsync(p => p.Providerinfoid == id);

        if (entity == null) return null;

        // Validate NPI unique if changing
        if (!string.IsNullOrEmpty(dto.NpiNumber) &&
            entity.Providerprofile?.NpiNumber != dto.NpiNumber)
        {
            var npiExists = await _context.Providerprofiles
                .AnyAsync(p => p.NpiNumber == dto.NpiNumber &&
                               p.Providerinfoid != id);
            if (npiExists)
                throw new ArgumentException(
                    $"NPI Number '{dto.NpiNumber}' already exists.");
        }

        // Read profile picture bytes
        var pictureBytes = await ReadProfilePictureAsync(dto.Profilepicture);

        // Update Providerinfo
        ProviderInfoMapper.UpdateProviderinfoEntity(entity, dto, pictureBytes);

        // Update Providerprofile if exists
        if (entity.Providerprofile != null)
        {
            ProviderInfoMapper.UpdateProviderprofileEntity(
                entity.Providerprofile, dto);
        }

        await _context.SaveChangesAsync();
        return ProviderInfoMapper.ToResponseDto(entity);
    }

    public async Task<ProviderProfilePictureResponseDto?> UpdateProfilePictureAsync(
        long id, IFormFile picture, long? updatedby)
    {
        var entity = await _context.Providerinfos
            .FirstOrDefaultAsync(p => p.Providerinfoid == id);

        if (entity == null) return null;

        var pictureBytes = await ReadProfilePictureAsync(picture);
        if (pictureBytes == null)
            throw new ArgumentException("Invalid picture file.");

        entity.Profilepicture = pictureBytes;
        entity.Updatedby = updatedby;
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();

        return new ProviderProfilePictureResponseDto
        {
            Providerinfoid = entity.Providerinfoid,
            Fullname = $"{entity.Firstname} {entity.Lastname}".Trim(),
            HasProfilePicture = true,
            ProfilepictureBase64 = Convert.ToBase64String(pictureBytes)
        };
    }

    public async Task<byte[]?> GetProfilePictureAsync(long id)
    {
        var entity = await _context.Providerinfos
            .FirstOrDefaultAsync(p => p.Providerinfoid == id);

        return entity?.Profilepicture;
    }

    public async Task<bool> DeactivateAsync(long id, long? updatedby)
    {
        var entity = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .FirstOrDefaultAsync(p => p.Providerinfoid == id);

        if (entity == null) return false;

        if (entity.Providerprofile != null)
        {
            entity.Providerprofile.Isactive = false;
            entity.Providerprofile.Updatedby = updatedby;
            entity.Providerprofile.Updatedat = DateTime.SpecifyKind(
                DateTime.UtcNow, DateTimeKind.Unspecified);
        }

        entity.Updatedby = updatedby;
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Providerinfos.FindAsync(id);
        if (entity == null) return false;

        _context.Providerinfos.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}