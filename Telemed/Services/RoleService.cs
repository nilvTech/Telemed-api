// Services/RoleService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class RoleService : IRoleService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidRoletypes = new[]
    {
        "Admin", "Provider", "Patient",
        "Biller", "Nurse", "CareManager",
        "Pharmacist", "Receptionist", "Auditor"
    };

    private static readonly string[] ValidAccesslevels = new[]
    {
        "Full", "High", "Medium", "Limited", "Audit"
    };

    private static readonly string[] ValidDatascopes = new[]
    {
        "All", "Own", "Team", "Limited"
    };

    private static readonly string[] ValidStatuses = new[]
    {
        "Active", "Inactive"
    };

    public RoleService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<RoleResponseDto> CreateAsync(CreateRoleDto dto)
    {
        // Validate rolecode unique
        var codeExists = await _context.Roles
            .AnyAsync(r => r.Rolecode.ToLower() ==
                           dto.Rolecode.ToLower());
        if (codeExists)
            throw new ArgumentException(
                $"Role code '{dto.Rolecode}' already exists.");

        // Validate roletype
        if (!ValidRoletypes.Contains(dto.Roletype,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Role Type. Allowed: " +
                $"{string.Join(", ", ValidRoletypes)}.");

        // Validate access level
        if (!ValidAccesslevels.Contains(dto.Accesslevel,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Access Level. Allowed: " +
                $"{string.Join(", ", ValidAccesslevels)}.");

        // Validate datascope
        if (!string.IsNullOrEmpty(dto.Datascope) &&
            !ValidDatascopes.Contains(dto.Datascope,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Data Scope. Allowed: " +
                $"{string.Join(", ", ValidDatascopes)}.");

        // Validate status
        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        var entity = RoleMapper.ToEntity(dto);
        _context.Roles.Add(entity);
        await _context.SaveChangesAsync();

        return RoleMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetAllAsync()
    {
        var list = await _context.Roles
            .OrderBy(r => r.Rolename)
            .ToListAsync();

        return list.Select(RoleMapper.ToResponseDto);
    }

    public async Task<RoleResponseDto?> GetByIdAsync(long id)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(r => r.Roleid == id);

        if (entity == null) return null;
        return RoleMapper.ToResponseDto(entity);
    }

    public async Task<RoleResponseDto?> GetByRolecodeAsync(string rolecode)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(r =>
                r.Rolecode.ToLower() == rolecode.ToLower());

        if (entity == null) return null;
        return RoleMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetByRoletypeAsync(
        string roletype)
    {
        var list = await _context.Roles
            .Where(r => r.Roletype.ToLower() == roletype.ToLower())
            .OrderBy(r => r.Rolename)
            .ToListAsync();

        return list.Select(RoleMapper.ToResponseDto);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetByAccesslevelAsync(
        string accesslevel)
    {
        var list = await _context.Roles
            .Where(r => r.Accesslevel.ToLower() ==
                        accesslevel.ToLower())
            .OrderBy(r => r.Rolename)
            .ToListAsync();

        return list.Select(RoleMapper.ToResponseDto);
    }

    public async Task<IEnumerable<RoleResponseDto>> GetActiveAsync()
    {
        var list = await _context.Roles
            .Where(r => r.Status == "Active")
            .OrderBy(r => r.Rolename)
            .ToListAsync();

        return list.Select(RoleMapper.ToResponseDto);
    }

    public async Task<RoleResponseDto?> UpdateAsync(
        long id, UpdateRoleDto dto)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(r => r.Roleid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Roletype) &&
            !ValidRoletypes.Contains(dto.Roletype,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Role Type. Allowed: " +
                $"{string.Join(", ", ValidRoletypes)}.");

        if (!string.IsNullOrEmpty(dto.Accesslevel) &&
            !ValidAccesslevels.Contains(dto.Accesslevel,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Access Level. Allowed: " +
                $"{string.Join(", ", ValidAccesslevels)}.");

        if (!string.IsNullOrEmpty(dto.Datascope) &&
            !ValidDatascopes.Contains(dto.Datascope,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Data Scope. Allowed: " +
                $"{string.Join(", ", ValidDatascopes)}.");

        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: " +
                $"{string.Join(", ", ValidStatuses)}.");

        RoleMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return RoleMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeactivateAsync(long id, long? updatedby)
    {
        var entity = await _context.Roles
            .FirstOrDefaultAsync(r => r.Roleid == id);

        if (entity == null) return false;

        entity.Status = "Inactive";
        entity.Updatedby = updatedby;
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Roles.FindAsync(id);
        if (entity == null) return false;

        // Check if users are assigned to this role
        var hasUsers = await _context.Users
            .AnyAsync(u => u.Role == entity.Rolecode);
        if (hasUsers)
            throw new ArgumentException(
                "Cannot delete role — it is assigned to one or more users.");

        _context.Roles.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // ===== Dashboard VIEW =====

    public async Task<IEnumerable<RolesDashboardResponseDto>> GetDashboardAsync()
    {
        var list = await _context.RolesDashboards
            .ToListAsync();

        return list.Select(RoleMapper.ToDashboardResponseDto);
    }

    public async Task<RolesDashboardResponseDto?> GetDashboardByIdAsync(long id)
    {
        var entity = await _context.RolesDashboards
            .FirstOrDefaultAsync(r => r.Roleid == id);

        if (entity == null) return null;
        return RoleMapper.ToDashboardResponseDto(entity);
    }
}