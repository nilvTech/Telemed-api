// Services/CareteamproviderService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class CareteamproviderService : ICareteamproviderService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidRoles = new[]
    {
        "PCP", "RN", "Care Manager", "Specialist",
        "Pharmacist", "Social Worker",
        "Physical Therapist", "Dietitian"
    };

    public CareteamproviderService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Careteamprovider> BaseQuery()
    {
        return _context.Careteamproviders
            .Include(c => c.Careteam)
            .Include(c => c.Providerinfo)
                .ThenInclude(p => p!.Providerprofile);
    }

    public async Task<CareteamproviderResponseDto> CreateAsync(
        CreateCareteamproviderDto dto)
    {
        // Validate Careteam exists
        var careteamExists = await _context.Careteams
            .AnyAsync(c => c.Careteamid == dto.Careteamid);
        if (!careteamExists)
            throw new ArgumentException(
                $"Care team with ID {dto.Careteamid} does not exist.");

        // Validate Provider exists
        var providerExists = await _context.Providerinfos
            .AnyAsync(p => p.Providerinfoid == dto.Providerinfoid);
        if (!providerExists)
            throw new ArgumentException(
                $"Provider with ID {dto.Providerinfoid} does not exist.");

        // Validate not already active member
        var alreadyAssigned = await _context.Careteamproviders
            .AnyAsync(c => c.Careteamid == dto.Careteamid &&
                           c.Providerinfoid == dto.Providerinfoid &&
                           c.Isactive == true);
        if (alreadyAssigned)
            throw new ArgumentException(
                "Provider is already an active member of this care team.");

        // Validate Role
        if (!string.IsNullOrEmpty(dto.Role) &&
            !ValidRoles.Contains(dto.Role, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Role. Allowed: {string.Join(", ", ValidRoles)}.");

        var entity = CareteamproviderMapper.ToEntity(dto);
        _context.Careteamproviders.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(c =>
                c.Careteamproviderid == entity.Careteamproviderid);

        return CareteamproviderMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<CareteamproviderResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteamproviderMapper.ToResponseDto);
    }

    public async Task<CareteamproviderResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careteamproviderid == id);

        if (entity == null) return null;
        return CareteamproviderMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<CareteamproviderResponseDto>> GetByCareteamIdAsync(
        long careteamId)
    {
        var list = await BaseQuery()
            .Where(c => c.Careteamid == careteamId)
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteamproviderMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteamproviderResponseDto>> GetByProviderIdAsync(
        long providerInfoId)
    {
        var list = await BaseQuery()
            .Where(c => c.Providerinfoid == providerInfoId)
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteamproviderMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteamproviderResponseDto>> GetByRoleAsync(
        string role)
    {
        var list = await BaseQuery()
            .Where(c => c.Role != null &&
                        c.Role.ToLower() == role.ToLower() &&
                        c.Isactive == true)
            .OrderBy(c => c.Providerinfo.Lastname)
            .ToListAsync();

        return list.Select(CareteamproviderMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteamproviderResponseDto>> GetActiveAsync()
    {
        var list = await BaseQuery()
            .Where(c => c.Isactive == true)
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteamproviderMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteamproviderResponseDto>> GetActiveByCareteamIdAsync(
        long careteamId)
    {
        var list = await BaseQuery()
            .Where(c => c.Careteamid == careteamId &&
                        c.Isactive == true)
            .OrderBy(c => c.Role)
            .ThenBy(c => c.Providerinfo.Lastname)
            .ToListAsync();

        return list.Select(CareteamproviderMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteamproviderResponseDto>> GetActiveByProviderIdAsync(
        long providerInfoId)
    {
        var list = await BaseQuery()
            .Where(c => c.Providerinfoid == providerInfoId &&
                        c.Isactive == true)
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteamproviderMapper.ToResponseDto);
    }

    public async Task<CareteamproviderResponseDto?> UpdateAsync(
        long id, UpdateCareteamproviderDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careteamproviderid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Role) &&
            !ValidRoles.Contains(dto.Role, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Role. Allowed: {string.Join(", ", ValidRoles)}.");

        CareteamproviderMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return CareteamproviderMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeactivateAsync(long id)
    {
        var entity = await _context.Careteamproviders
            .FirstOrDefaultAsync(c => c.Careteamproviderid == id);

        if (entity == null) return false;

        entity.Isactive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Careteamproviders.FindAsync(id);
        if (entity == null) return false;

        _context.Careteamproviders.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}