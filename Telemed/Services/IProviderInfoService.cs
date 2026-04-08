// Services/Interfaces/IProviderInfoService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IProviderInfoService
{
    // Create provider with profile in one call
    Task<ProviderInfoResponseDto> CreateAsync(CreateProviderInfoDto dto);

    // Get all providers with profiles
    Task<IEnumerable<ProviderInfoResponseDto>> GetAllAsync();

    // Get by ID
    Task<ProviderInfoResponseDto?> GetByIdAsync(long id);

    // Get by Email
    Task<ProviderInfoResponseDto?> GetByEmailAsync(string email);

    // Get by Speciality
    Task<IEnumerable<ProviderInfoResponseDto>> GetBySpecialityAsync(string speciality);

    // Get by State — US telemedicine license check
    Task<IEnumerable<ProviderInfoResponseDto>> GetByStateAsync(string state);

    // Get active providers only
    Task<IEnumerable<ProviderInfoResponseDto>> GetActiveAsync();

    // Update provider + profile in one call
    Task<ProviderInfoResponseDto?> UpdateAsync(long id, UpdateProviderInfoDto dto);

    // Update profile picture only
    Task<ProviderProfilePictureResponseDto?> UpdateProfilePictureAsync(
        long id, IFormFile picture, long? updatedby);

    // Get profile picture only
    Task<byte[]?> GetProfilePictureAsync(long id);

    // Deactivate provider
    Task<bool> DeactivateAsync(long id, long? updatedby);

    // Delete provider
    Task<bool> DeleteAsync(long id);
}