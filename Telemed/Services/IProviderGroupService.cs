// Services/Interfaces/IProviderGroupService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IProviderGroupService
{
    // ===== Group CRUD =====
    Task<ProviderGroupResponseDto> CreateGroupAsync(CreateProviderGroupDto dto);
    Task<IEnumerable<ProviderGroupResponseDto>> GetAllGroupsAsync();
    Task<ProviderGroupResponseDto?> GetGroupByIdAsync(long groupId);
    Task<IEnumerable<ProviderGroupResponseDto>> GetGroupsBySpecialityAsync(
        string speciality);
    Task<IEnumerable<ProviderGroupResponseDto>> GetGroupsByStateAsync(string state);
    Task<IEnumerable<ProviderGroupResponseDto>> GetActiveGroupsAsync();
    Task<ProviderGroupResponseDto?> UpdateGroupAsync(
        long groupId, UpdateProviderGroupDto dto);
    Task<bool> DeactivateGroupAsync(long groupId, long? updatedby);
    Task<bool> DeleteGroupAsync(long groupId);

    // ===== Member CRUD =====
    Task<ProviderGroupMemberResponseDto> AddMemberAsync(
        AddProviderGroupMemberDto dto);
    Task<IEnumerable<ProviderGroupMemberResponseDto>> GetMembersByGroupIdAsync(
        long groupId);
    Task<IEnumerable<ProviderGroupMemberResponseDto>> GetGroupsByProviderIdAsync(
        long providerInfoId);
    Task<ProviderGroupMemberResponseDto?> GetMemberByIdAsync(long memberId);
    Task<ProviderGroupMemberResponseDto?> UpdateMemberAsync(
        long memberId, UpdateProviderGroupMemberDto dto);
    Task<bool> RemoveMemberAsync(long memberId, long? updatedby);
}