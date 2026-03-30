using Telemed.DTOs;

namespace Telemed.Services.Interfaces
{
    public interface IVideoSessionService
    {
        Task<IEnumerable<VideoSessionResponseDto>> GetAllAsync();
        Task<VideoSessionResponseDto?> GetByIdAsync(int id);
        Task<VideoSessionResponseDto> CreateAsync(CreateVideoSessionDto dto);
        Task<VideoSessionResponseDto?> UpdateAsync(int id, UpdateVideoSessionDto dto);
        Task<bool> DeleteAsync(int id);
    }
}