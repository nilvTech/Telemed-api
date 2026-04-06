using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IProviderService
{
    Task<List<ProviderDto>> GetAllAsync();
    Task<ProviderDto?> GetByIdAsync(long id);         // long
    Task<ProviderDto> CreateAsync(CreateProviderDto dto);
    Task<ProviderDto?> UpdateAsync(long id, UpdateProviderDto dto); // long
    Task<bool> DeleteAsync(long id);                 // long
}