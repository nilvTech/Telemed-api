// Services/Interfaces/ICareteamproviderService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface ICareteamproviderService
{
    Task<CareteamproviderResponseDto> CreateAsync(CreateCareteamproviderDto dto);
    Task<IEnumerable<CareteamproviderResponseDto>> GetAllAsync();
    Task<CareteamproviderResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<CareteamproviderResponseDto>> GetByCareteamIdAsync(long careteamId);
    Task<IEnumerable<CareteamproviderResponseDto>> GetByProviderIdAsync(long providerInfoId);
    Task<IEnumerable<CareteamproviderResponseDto>> GetByRoleAsync(string role);
    Task<IEnumerable<CareteamproviderResponseDto>> GetActiveAsync();
    Task<IEnumerable<CareteamproviderResponseDto>> GetActiveByCareteamIdAsync(long careteamId);
    Task<IEnumerable<CareteamproviderResponseDto>> GetActiveByProviderIdAsync(long providerInfoId);
    Task<CareteamproviderResponseDto?> UpdateAsync(long id, UpdateCareteamproviderDto dto);
    Task<bool> DeactivateAsync(long id);
    Task<bool> DeleteAsync(long id);
}