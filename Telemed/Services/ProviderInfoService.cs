using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ProviderInfoService : IProviderInfoService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidImageTypes =
        { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
    private const long MaxPictureSize = 5 * 1024 * 1024; // 5MB

    public ProviderInfoService(TelemedDbContext context)
    {
        _context = context;
    }

    private async Task<byte[]?> ReadProfilePictureAsync(IFormFile? file)
    {
        if (file == null || file.Length == 0) return null;

        if (!ValidImageTypes.Contains(file.ContentType.ToLower()))
            throw new ArgumentException("Invalid image type. Allowed: JPEG, PNG, GIF, WebP.");

        if (file.Length > MaxPictureSize)
            throw new ArgumentException("Profile picture cannot exceed 5MB.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task<ProviderInfoResponseDto> CreateAsync(CreateProviderInfoDto dto)
    {
        if (await _context.Providerinfos.AnyAsync(p => p.Email == dto.Email))
            throw new ArgumentException($"Email '{dto.Email}' is already registered.");

        if (!string.IsNullOrEmpty(dto.NpiNumber) &&
            await _context.Providerprofiles.AnyAsync(p => p.NpiNumber == dto.NpiNumber))
            throw new ArgumentException($"NPI Number '{dto.NpiNumber}' already exists.");

        var pictureBytes = await ReadProfilePictureAsync(dto.Profilepicture);

        var providerinfo = ProviderInfoMapper.ToProviderinfoEntity(dto, pictureBytes);
        _context.Providerinfos.Add(providerinfo);
        await _context.SaveChangesAsync();

        var providerprofile = ProviderInfoMapper.ToProviderprofileEntity(dto, providerinfo.Providerinfoid);
        _context.Providerprofiles.Add(providerprofile);
        await _context.SaveChangesAsync();

        var created = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .FirstAsync(p => p.Providerinfoid == providerinfo.Providerinfoid);

        return ProviderInfoMapper.ToResponseDto(created);
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

        return entity == null ? null : ProviderInfoMapper.ToResponseDto(entity);
    }

    public async Task<ProviderInfoResponseDto?> GetByEmailAsync(string email)
    {
        var entity = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .FirstOrDefaultAsync(p => p.Email.ToLower() == email.ToLower());

        return entity == null ? null : ProviderInfoMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<ProviderInfoResponseDto>> GetBySpecialityAsync(string speciality)
    {
        var list = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .Where(p => p.Providerprofile != null &&
                        (p.Providerprofile.Speciality1.ToLower().Contains(speciality.ToLower()) ||
                         (p.Providerprofile.Speciality2 != null &&
                          p.Providerprofile.Speciality2.ToLower().Contains(speciality.ToLower()))))
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(ProviderInfoMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ProviderInfoResponseDto>> GetByStateAsync(string state)
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
            .Where(p => p.Providerprofile != null && p.Providerprofile.Isactive == true)
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(ProviderInfoMapper.ToResponseDto);
    }

    public async Task<ProviderInfoResponseDto?> UpdateAsync(long id, UpdateProviderInfoDto dto)
    {
        var entity = await _context.Providerinfos
            .Include(p => p.Providerprofile)
            .FirstOrDefaultAsync(p => p.Providerinfoid == id);

        if (entity == null) return null;

        var pictureBytes = await ReadProfilePictureAsync(dto.Profilepicture);

        // Update Providerinfo
        if (!string.IsNullOrEmpty(dto.GroupName)) entity.GroupName = dto.GroupName;
        if (!string.IsNullOrEmpty(dto.Firstname)) entity.Firstname = dto.Firstname;
        if (!string.IsNullOrEmpty(dto.Lastname)) entity.Lastname = dto.Lastname;
        if (!string.IsNullOrEmpty(dto.Phone)) entity.Phone = dto.Phone;
        if (!string.IsNullOrEmpty(dto.Gender)) entity.Gender = dto.Gender;
        if (!string.IsNullOrEmpty(dto.Displayname)) entity.Displayname = dto.Displayname;
        if (pictureBytes != null) entity.Profilepicture = pictureBytes;
        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        // Update Providerprofile
        if (entity.Providerprofile != null)
        {
            if (!string.IsNullOrEmpty(dto.Providertype)) entity.Providerprofile.Providertype = dto.Providertype;
            if (!string.IsNullOrEmpty(dto.Bio)) entity.Providerprofile.Bio = dto.Bio;
            if (dto.Yearofexperience.HasValue) entity.Providerprofile.Yearofexperience = dto.Yearofexperience;
            if (!string.IsNullOrEmpty(dto.Licensenumber)) entity.Providerprofile.Licensenumber = dto.Licensenumber;
            if (!string.IsNullOrEmpty(dto.NpiNumber)) entity.Providerprofile.NpiNumber = dto.NpiNumber;
            if (!string.IsNullOrEmpty(dto.Secondaryrole)) entity.Providerprofile.Secondaryrole = dto.Secondaryrole;
            if (!string.IsNullOrEmpty(dto.Speciality1)) entity.Providerprofile.Speciality1 = dto.Speciality1;
            if (!string.IsNullOrEmpty(dto.Speciality2)) entity.Providerprofile.Speciality2 = dto.Speciality2;
            if (!string.IsNullOrEmpty(dto.Website)) entity.Providerprofile.Website = dto.Website;
            if (!string.IsNullOrEmpty(dto.Timezone)) entity.Providerprofile.Timezone = dto.Timezone;
            if (!string.IsNullOrEmpty(dto.Addressline1)) entity.Providerprofile.Addressline1 = dto.Addressline1;
            if (!string.IsNullOrEmpty(dto.Addressline2)) entity.Providerprofile.Addressline2 = dto.Addressline2;
            if (!string.IsNullOrEmpty(dto.City)) entity.Providerprofile.City = dto.City;
            if (!string.IsNullOrEmpty(dto.State)) entity.Providerprofile.State = dto.State;
            if (!string.IsNullOrEmpty(dto.Zip)) entity.Providerprofile.Zip = dto.Zip;
            if (!string.IsNullOrEmpty(dto.Country)) entity.Providerprofile.Country = dto.Country;
            if (dto.Isactive.HasValue) entity.Providerprofile.Isactive = dto.Isactive;

            entity.Providerprofile.Updatedby = dto.Updatedby;
            entity.Providerprofile.Updatedat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        }

        await _context.SaveChangesAsync();
        return ProviderInfoMapper.ToResponseDto(entity);
    }

    public async Task<ProviderProfilePictureResponseDto?> UpdateProfilePictureAsync(
        long id, IFormFile picture, long? updatedby)
    {
        var entity = await _context.Providerinfos.FirstOrDefaultAsync(p => p.Providerinfoid == id);
        if (entity == null) return null;

        var pictureBytes = await ReadProfilePictureAsync(picture);
        if (pictureBytes == null) throw new ArgumentException("Invalid picture.");

        entity.Profilepicture = pictureBytes;
        entity.Updatedby = updatedby;
        entity.Updatedat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

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
        var entity = await _context.Providerinfos.FirstOrDefaultAsync(p => p.Providerinfoid == id);
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
            entity.Providerprofile.Updatedat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        }

        entity.Updatedby = updatedby;
        entity.Updatedat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

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