// Services/CareteamService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class CareteamService : ICareteamService
{
    private readonly TelemedDbContext _context;

    public CareteamService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<CareteamResponseDto> CreateAsync(CreateCareteamDto dto)
    {
        // Validate team name not empty
        if (string.IsNullOrWhiteSpace(dto.Teamname))
            throw new ArgumentException("Team name cannot be empty.");

        // Validate team name unique
        var exists = await _context.Careteams
            .AnyAsync(c => c.Teamname.ToLower() == dto.Teamname.ToLower());
        if (exists)
            throw new ArgumentException(
                $"A care team named '{dto.Teamname}' already exists.");

        var entity = CareteamMapper.ToEntity(dto);
        _context.Careteams.Add(entity);
        await _context.SaveChangesAsync();

        return CareteamMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<CareteamResponseDto>> GetAllAsync()
    {
        var list = await _context.Careteams
            .OrderBy(c => c.Teamname)
            .ToListAsync();

        return list.Select(CareteamMapper.ToResponseDto);
    }

    public async Task<CareteamResponseDto?> GetByIdAsync(long id)
    {
        var entity = await _context.Careteams
            .FirstOrDefaultAsync(c => c.Careteamid == id);

        if (entity == null) return null;
        return CareteamMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<CareteamResponseDto>> SearchAsync(string keyword)
    {
        var list = await _context.Careteams
            .Where(c =>
                c.Teamname.ToLower().Contains(keyword.ToLower()) ||
                (c.Description != null &&
                 c.Description.ToLower().Contains(keyword.ToLower())))
            .OrderBy(c => c.Teamname)
            .ToListAsync();

        return list.Select(CareteamMapper.ToResponseDto);
    }

    public async Task<CareteamResponseDto?> UpdateAsync(
        long id, UpdateCareteamDto dto)
    {
        var entity = await _context.Careteams
            .FirstOrDefaultAsync(c => c.Careteamid == id);

        if (entity == null) return null;

        // Validate team name unique if changing
        if (!string.IsNullOrEmpty(dto.Teamname) &&
            dto.Teamname.ToLower() != entity.Teamname.ToLower())
        {
            var exists = await _context.Careteams
                .AnyAsync(c => c.Teamname.ToLower() ==
                               dto.Teamname.ToLower() &&
                               c.Careteamid != id);
            if (exists)
                throw new ArgumentException(
                    $"A care team named '{dto.Teamname}' already exists.");
        }

        CareteamMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return CareteamMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Careteams.FindAsync(id);
        if (entity == null) return false;

        _context.Careteams.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}