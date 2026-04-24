// Services/Interfaces/ICareteamService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface ICareteamService
{
    Task<CareteamResponseDto> CreateAsync(CreateCareteamDto dto);
    Task<IEnumerable<CareteamResponseDto>> GetAllAsync();
    Task<CareteamResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<CareteamResponseDto>> SearchAsync(string keyword);
    Task<CareteamResponseDto?> UpdateAsync(long id, UpdateCareteamDto dto);
    Task<bool> DeleteAsync(long id);
}