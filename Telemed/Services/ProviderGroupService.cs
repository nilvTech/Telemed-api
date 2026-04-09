using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ProviderGroupService : IProviderGroupService
{
    private readonly TelemedDbContext _context;

    public ProviderGroupService(TelemedDbContext context)
    {
        _context = context;
    }

    // ===================== GROUP CRUD =====================

    public async Task<ProviderGroupResponseDto> CreateGroupAsync(CreateProviderGroupDto dto)
    {
        var emailExists = await _context.ProviderGroups
            .AnyAsync(g => g.Email == dto.Email);

        if (emailExists)
            throw new ArgumentException($"A group with email '{dto.Email}' already exists.");

        var entity = ProviderGroupMapper.ToEntity(dto);

        _context.ProviderGroups.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.ProviderGroups
            .Include(g => g.GroupMembers)
                .ThenInclude(m => m.ProviderInfo)
                    .ThenInclude(p => p.Providerprofile)
            .FirstOrDefaultAsync(g => g.GroupId == entity.GroupId);

        return ProviderGroupMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<ProviderGroupResponseDto>> GetAllGroupsAsync()
    {
        var list = await _context.ProviderGroups
            .Include(g => g.GroupMembers)
                .ThenInclude(m => m.ProviderInfo)
                    .ThenInclude(p => p.Providerprofile)
            .OrderBy(g => g.GroupName)
            .ToListAsync();

        return list.Select(ProviderGroupMapper.ToResponseDto);
    }

    public async Task<ProviderGroupResponseDto?> GetGroupByIdAsync(long groupId)
    {
        var entity = await _context.ProviderGroups
            .Include(g => g.GroupMembers)
                .ThenInclude(m => m.ProviderInfo)
                    .ThenInclude(p => p.Providerprofile)
            .FirstOrDefaultAsync(g => g.GroupId == groupId);

        return entity == null ? null : ProviderGroupMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<ProviderGroupResponseDto>> GetGroupsBySpecialityAsync(string speciality)
    {
        var list = await _context.ProviderGroups
            .Include(g => g.GroupMembers)
                .ThenInclude(m => m.ProviderInfo)
                    .ThenInclude(p => p.Providerprofile)
            .Where(g => g.Speciality != null &&
                        g.Speciality.ToLower().Contains(speciality.ToLower()) &&
                        g.IsActive == true)
            .OrderBy(g => g.GroupName)
            .ToListAsync();

        return list.Select(ProviderGroupMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ProviderGroupResponseDto>> GetGroupsByStateAsync(string state)
    {
        var list = await _context.ProviderGroups
            .Include(g => g.GroupMembers)
                .ThenInclude(m => m.ProviderInfo)
                    .ThenInclude(p => p.Providerprofile)
            .Where(g => g.State.ToLower() == state.ToLower() &&
                        g.IsActive == true)
            .OrderBy(g => g.GroupName)
            .ToListAsync();

        return list.Select(ProviderGroupMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ProviderGroupResponseDto>> GetActiveGroupsAsync()
    {
        var list = await _context.ProviderGroups
            .Include(g => g.GroupMembers)
                .ThenInclude(m => m.ProviderInfo)
                    .ThenInclude(p => p.Providerprofile)
            .Where(g => g.IsActive == true)
            .OrderBy(g => g.GroupName)
            .ToListAsync();

        return list.Select(ProviderGroupMapper.ToResponseDto);
    }

    public async Task<ProviderGroupResponseDto?> UpdateGroupAsync(long groupId, UpdateProviderGroupDto dto)
    {
        var entity = await _context.ProviderGroups
            .Include(g => g.GroupMembers)
                .ThenInclude(m => m.ProviderInfo)
                    .ThenInclude(p => p.Providerprofile)
            .FirstOrDefaultAsync(g => g.GroupId == groupId);

        if (entity == null) return null;

        ProviderGroupMapper.UpdateEntity(entity, dto);

        await _context.SaveChangesAsync();
        return ProviderGroupMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeactivateGroupAsync(long groupId, long? updatedby)
    {
        var entity = await _context.ProviderGroups.FirstOrDefaultAsync(g => g.GroupId == groupId);

        if (entity == null) return false;

        entity.IsActive = false;

        // Group has audit fields — keep them
        entity.UpdatedBy = updatedby;
        entity.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteGroupAsync(long groupId)
    {
        var entity = await _context.ProviderGroups.FindAsync(groupId);
        if (entity == null) return false;

        _context.ProviderGroups.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // ===================== MEMBER CRUD =====================

    public async Task<ProviderGroupMemberResponseDto> AddMemberAsync(AddProviderGroupMemberDto dto)
    {
        var groupExists = await _context.ProviderGroups.AnyAsync(g => g.GroupId == dto.GroupId);
        if (!groupExists)
            throw new ArgumentException($"Group with ID {dto.GroupId} does not exist.");

        var providerExists = await _context.Providerinfos.AnyAsync(p => p.Providerinfoid == dto.ProviderInfoId);
        if (!providerExists)
            throw new ArgumentException($"Provider with ID {dto.ProviderInfoId} does not exist.");

        var alreadyMember = await _context.ProviderGroupMembers
            .AnyAsync(m => m.GroupId == dto.GroupId &&
                           m.ProviderInfoId == dto.ProviderInfoId &&
                           m.IsActive == true);

        if (alreadyMember)
            throw new ArgumentException("This provider is already an active member of this group.");

        var validRoles = new[] { "Member", "Lead", "Admin", "Director", "Coordinator", "Supervisor" };

        if (!string.IsNullOrEmpty(dto.RoleInGroup) &&
            !validRoles.Contains(dto.RoleInGroup, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Invalid RoleInGroup.");

        var entity = ProviderGroupMapper.ToMemberEntity(dto);

        _context.ProviderGroupMembers.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.ProviderGroupMembers
            .Include(m => m.Group)
            .Include(m => m.ProviderInfo)
                .ThenInclude(p => p.Providerprofile)
            .FirstOrDefaultAsync(m => m.GroupMemberId == entity.GroupMemberId);

        return ProviderGroupMapper.ToMemberResponseDto(created!);
    }

    public async Task<IEnumerable<ProviderGroupMemberResponseDto>> GetMembersByGroupIdAsync(long groupId)
    {
        var list = await _context.ProviderGroupMembers
            .Include(m => m.Group)
            .Include(m => m.ProviderInfo)
                .ThenInclude(p => p.Providerprofile)
            .Where(m => m.GroupId == groupId)
            .OrderBy(m => m.ProviderInfo.Lastname)
            .ToListAsync();

        return list.Select(ProviderGroupMapper.ToMemberResponseDto);
    }

    public async Task<IEnumerable<ProviderGroupMemberResponseDto>> GetGroupsByProviderIdAsync(long providerInfoId)
    {
        var list = await _context.ProviderGroupMembers
            .Include(m => m.Group)
            .Include(m => m.ProviderInfo)
                .ThenInclude(p => p.Providerprofile)
            .Where(m => m.ProviderInfoId == providerInfoId)
            .OrderBy(m => m.Group.GroupName)
            .ToListAsync();

        return list.Select(ProviderGroupMapper.ToMemberResponseDto);
    }

    public async Task<ProviderGroupMemberResponseDto?> GetMemberByIdAsync(long memberId)
    {
        var entity = await _context.ProviderGroupMembers
            .Include(m => m.Group)
            .Include(m => m.ProviderInfo)
                .ThenInclude(p => p.Providerprofile)
            .FirstOrDefaultAsync(m => m.GroupMemberId == memberId);

        return entity == null ? null : ProviderGroupMapper.ToMemberResponseDto(entity);
    }

    public async Task<ProviderGroupMemberResponseDto?> UpdateMemberAsync(long memberId, UpdateProviderGroupMemberDto dto)
    {
        var entity = await _context.ProviderGroupMembers
            .Include(m => m.Group)
            .Include(m => m.ProviderInfo)
                .ThenInclude(p => p.Providerprofile)
            .FirstOrDefaultAsync(m => m.GroupMemberId == memberId);

        if (entity == null) return null;

        var validRoles = new[] { "Member", "Lead", "Admin", "Director", "Coordinator", "Supervisor" };

        if (!string.IsNullOrEmpty(dto.RoleInGroup) &&
            !validRoles.Contains(dto.RoleInGroup, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Invalid RoleInGroup.");

        ProviderGroupMapper.UpdateMemberEntity(entity, dto);

        await _context.SaveChangesAsync();
        return ProviderGroupMapper.ToMemberResponseDto(entity);
    }

    public async Task<bool> RemoveMemberAsync(long memberId, long? updatedby)
    {
        var entity = await _context.ProviderGroupMembers
            .FirstOrDefaultAsync(m => m.GroupMemberId == memberId);

        if (entity == null) return false;

        // Soft delete only — NO audit fields on member
        entity.IsActive = false;

        await _context.SaveChangesAsync();
        return true;
    }
}