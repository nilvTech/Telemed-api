using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IProviderService
{
    Task<List<ProviderDto>> GetAllAsync();
    Task<ProviderDto?> GetByIdAsync(int id);
    Task<ProviderDto> CreateAsync(CreateProviderDto dto);
    Task<ProviderDto?> UpdateAsync(int id, UpdateProviderDto dto);
    Task<bool> DeleteAsync(int id);
}