// Services/Interfaces/IRoleService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IRoleService
{
    // Role CRUD
    Task<RoleResponseDto> CreateAsync(CreateRoleDto dto);
    Task<IEnumerable<RoleResponseDto>> GetAllAsync();
    Task<RoleResponseDto?> GetByIdAsync(long id);
    Task<RoleResponseDto?> GetByRolecodeAsync(string rolecode);
    Task<IEnumerable<RoleResponseDto>> GetByRoletypeAsync(string roletype);
    Task<IEnumerable<RoleResponseDto>> GetByAccesslevelAsync(string accesslevel);
    Task<IEnumerable<RoleResponseDto>> GetActiveAsync();
    Task<RoleResponseDto?> UpdateAsync(long id, UpdateRoleDto dto);
    Task<bool> DeactivateAsync(long id, long? updatedby);
    Task<bool> DeleteAsync(long id);

    // Dashboard VIEW
    Task<IEnumerable<RolesDashboardResponseDto>> GetDashboardAsync();
    Task<RolesDashboardResponseDto?> GetDashboardByIdAsync(long id);
}