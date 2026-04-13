// Services/Interfaces/IConditionMasterService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IConditionMasterService
{
    Task<ConditionMasterResponseDto> CreateAsync(CreateConditionMasterDto dto);
    Task<IEnumerable<ConditionMasterResponseDto>> GetAllAsync();
    Task<ConditionMasterResponseDto?> GetByIdAsync(long id);
    Task<ConditionMasterResponseDto?> GetByIcdCodeAsync(string icdCode);
    Task<IEnumerable<ConditionMasterResponseDto>> GetByTypeAsync(string type);
    Task<IEnumerable<ConditionMasterResponseDto>> SearchAsync(string keyword);
    Task<ConditionMasterResponseDto?> UpdateAsync(long id, UpdateConditionMasterDto dto);
    Task<bool> DeleteAsync(long id);
}