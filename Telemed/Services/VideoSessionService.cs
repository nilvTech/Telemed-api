using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services
{
    public class VideoSessionService : IVideoSessionService
    {
        private readonly TelemedDbContext _context;

        public VideoSessionService(TelemedDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<VideoSessionResponseDto>> GetAllAsync()
        {
            var list = await _context.Videosessions.ToListAsync();
            return list.Select(VideoSessionMapper.ToDto);
        }

        public async Task<VideoSessionResponseDto?> GetByIdAsync(int id)
        {
            var entity = await _context.Videosessions.FindAsync(id);
            return entity == null ? null : VideoSessionMapper.ToDto(entity);
        }

        public async Task<VideoSessionResponseDto> CreateAsync(CreateVideoSessionDto dto)
        {
            var entity = VideoSessionMapper.ToEntity(dto);

            _context.Videosessions.Add(entity);
            await _context.SaveChangesAsync();

            return VideoSessionMapper.ToDto(entity);
        }

        public async Task<VideoSessionResponseDto?> UpdateAsync(int id, UpdateVideoSessionDto dto)
        {
            var entity = await _context.Videosessions.FindAsync(id);
            if (entity == null) return null;

            VideoSessionMapper.UpdateEntity(entity, dto);
            await _context.SaveChangesAsync();

            return VideoSessionMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Videosessions.FindAsync(id);
            if (entity == null) return false;

            _context.Videosessions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}