using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PatientConditionService : IPatientConditionService
{
    private readonly TelemedDbContext _context;

    public PatientConditionService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<PatientCondition> BaseQuery()
    {
        return _context.PatientConditions
            .Include(pc => pc.Patient)
            .Include(pc => pc.ConditionMaster)
            .Include(pc => pc.ProviderInfo);
          //  .Include(pc => pc.Consultation)  // 🔥 CRITICAL
    }

    public async Task<PatientConditionResponseDto> CreateAsync(CreatePatientConditionDto dto)
    {
        if (!await _context.Patients.AnyAsync(p => p.Patientid == dto.PatientId))
            throw new ArgumentException($"Patient ID {dto.PatientId} does not exist.");

        if (!await _context.ConditionMasters.AnyAsync(c => c.ConditionId == dto.ConditionId))
            throw new ArgumentException($"Condition ID {dto.ConditionId} does not exist.");

        var validStatuses = new[] { "Active", "Resolved", "Managed", "Inactive", "Suspected" };

        if (!string.IsNullOrWhiteSpace(dto.Status) &&
            !validStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Invalid Status.");

        var entity = PatientConditionMapper.ToEntity(dto);

        _context.PatientConditions.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstAsync(pc => pc.PatientConditionId == entity.PatientConditionId);

        return PatientConditionMapper.ToResponseDto(created);
    }

    public async Task<IEnumerable<PatientConditionResponseDto>> GetAllAsync()
    {
        return (await BaseQuery()
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync())
            .Select(PatientConditionMapper.ToResponseDto);
    }

    public async Task<PatientConditionResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(pc => pc.PatientConditionId == id);

        return entity == null ? null : PatientConditionMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<PatientConditionResponseDto>> GetByPatientIdAsync(long patientId)
    {
        return (await BaseQuery()
            .Where(pc => pc.PatientId == patientId)
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync())
            .Select(PatientConditionMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientConditionResponseDto>> GetActiveByPatientIdAsync(long patientId)
    {
        return (await BaseQuery()
            .Where(pc => pc.PatientId == patientId && pc.Status == "Active")
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync())
            .Select(PatientConditionMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientConditionResponseDto>> GetByConsultationIdAsync(long consultationId)
    {
        return (await BaseQuery()
            .Where(pc => pc.ConsultationId == consultationId)
            .ToListAsync())
            .Select(PatientConditionMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientConditionResponseDto>> GetByProviderIdAsync(long providerInfoId)
    {
        return (await BaseQuery()
            .Where(pc => pc.ProviderInfoId == providerInfoId)
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync())
            .Select(PatientConditionMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientConditionResponseDto>> GetByStatusAsync(string status)
    {
        return (await BaseQuery()
            .Where(pc => pc.Status.ToLower() == status.ToLower())
            .OrderByDescending(pc => pc.CreatedAt)
            .ToListAsync())
            .Select(PatientConditionMapper.ToResponseDto);
    }

    public async Task<PatientConditionResponseDto?> UpdateAsync(long id, UpdatePatientConditionDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(pc => pc.PatientConditionId == id);

        if (entity == null)
            return null;

        PatientConditionMapper.UpdateEntity(entity, dto);

        await _context.SaveChangesAsync();

        var updated = await BaseQuery()
            .FirstAsync(pc => pc.PatientConditionId == id);

        return PatientConditionMapper.ToResponseDto(updated);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.PatientConditions.FindAsync(id);

        if (entity == null)
            return false;

        _context.PatientConditions.Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }
}