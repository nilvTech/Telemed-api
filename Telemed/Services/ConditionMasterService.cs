// Services/ConditionMasterService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ConditionMasterService : IConditionMasterService
{
    private readonly TelemedDbContext _context;

    public ConditionMasterService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<ConditionMasterResponseDto> CreateAsync(
        CreateConditionMasterDto dto)
    {
        // Validate ICD code unique
        var icdExists = await _context.ConditionMasters
            .AnyAsync(c => c.IcdCode == dto.IcdCode);
        if (icdExists)
            throw new ArgumentException(
                $"ICD code '{dto.IcdCode}' already exists.");

        // Validate type
        if (!string.IsNullOrEmpty(dto.Type))
        {
            var validTypes = new[]
            {
                "Chronic", "Acute", "Mental Health",
                "Infectious", "Genetic", "Autoimmune",
                "Neurological", "Cardiovascular", "Other"
            };
            if (!validTypes.Contains(dto.Type,
                StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(
                    "Invalid Type. Allowed: Chronic, Acute, Mental Health, " +
                    "Infectious, Genetic, Autoimmune, Neurological, " +
                    "Cardiovascular, Other.");
        }

        var entity = ConditionMasterMapper.ToEntity(dto);
        _context.ConditionMasters.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.ConditionMasters
            .Include(c => c.PatientConditions)
            .FirstOrDefaultAsync(c => c.ConditionId == entity.ConditionId);

        return ConditionMasterMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<ConditionMasterResponseDto>> GetAllAsync()
    {
        var list = await _context.ConditionMasters
            .Include(c => c.PatientConditions)
            .OrderBy(c => c.ConditionName)
            .ToListAsync();

        return list.Select(ConditionMasterMapper.ToResponseDto);
    }

    public async Task<ConditionMasterResponseDto?> GetByIdAsync(long id)
    {
        var entity = await _context.ConditionMasters
            .Include(c => c.PatientConditions)
            .FirstOrDefaultAsync(c => c.ConditionId == id);

        if (entity == null) return null;
        return ConditionMasterMapper.ToResponseDto(entity);
    }

    public async Task<ConditionMasterResponseDto?> GetByIcdCodeAsync(
        string icdCode)
    {
        var entity = await _context.ConditionMasters
            .Include(c => c.PatientConditions)
            .FirstOrDefaultAsync(c =>
                c.IcdCode.ToLower() == icdCode.ToLower());

        if (entity == null) return null;
        return ConditionMasterMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<ConditionMasterResponseDto>> GetByTypeAsync(
        string type)
    {
        var list = await _context.ConditionMasters
            .Include(c => c.PatientConditions)
            .Where(c => c.Type != null &&
                        c.Type.ToLower() == type.ToLower())
            .OrderBy(c => c.ConditionName)
            .ToListAsync();

        return list.Select(ConditionMasterMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConditionMasterResponseDto>> SearchAsync(
        string keyword)
    {
        var list = await _context.ConditionMasters
            .Include(c => c.PatientConditions)
            .Where(c => c.ConditionName.ToLower()
                         .Contains(keyword.ToLower()) ||
                        c.IcdCode.ToLower()
                         .Contains(keyword.ToLower()) ||
                        (c.Description != null &&
                         c.Description.ToLower()
                          .Contains(keyword.ToLower())))
            .OrderBy(c => c.ConditionName)
            .ToListAsync();

        return list.Select(ConditionMasterMapper.ToResponseDto);
    }

    public async Task<ConditionMasterResponseDto?> UpdateAsync(
        long id, UpdateConditionMasterDto dto)
    {
        var entity = await _context.ConditionMasters
            .Include(c => c.PatientConditions)
            .FirstOrDefaultAsync(c => c.ConditionId == id);

        if (entity == null) return null;

        ConditionMasterMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ConditionMasterMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.ConditionMasters.FindAsync(id);
        if (entity == null) return false;

        // Check no patients assigned
        var hasPatients = await _context.PatientConditions
            .AnyAsync(pc => pc.ConditionId == id);
        if (hasPatients)
            throw new ArgumentException(
                "Cannot delete condition — it is assigned to one or more patients.");

        _context.ConditionMasters.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}