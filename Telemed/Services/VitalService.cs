using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class VitalService : IVitalService
{
    private readonly TelemedDbContext _context;

    public VitalService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<VitalResponseDto> CreateAsync(CreateVitalDto dto)
    {
        // Validate Encounter exists
        var encounterExists = await _context.Encounters
            .AnyAsync(e => e.Encounterid == dto.Encounterid);
        if (!encounterExists)
            throw new ArgumentException($"Encounter with ID {dto.Encounterid} does not exist.");

        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException($"Patient with ID {dto.Patientid} does not exist.");

        var entity = VitalMapper.ToEntity(dto);
        _context.Vitals.Add(entity);
        await _context.SaveChangesAsync();

        // Reload with related data for full response
        var created = await _context.Vitals
            .Include(v => v.Patient)
            .Include(v => v.Encounter)
            .FirstOrDefaultAsync(v => v.Vitalsid == entity.Vitalsid);

        return VitalMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<VitalResponseDto>> GetAllAsync()
    {
        var list = await _context.Vitals
            .Include(v => v.Patient)
            .Include(v => v.Encounter)
            .OrderByDescending(v => v.Recordedat)
            .ToListAsync();

        return list.Select(VitalMapper.ToResponseDto);
    }

    public async Task<VitalResponseDto?> GetByIdAsync(int id)
    {
        var vital = await _context.Vitals
            .Include(v => v.Patient)
            .Include(v => v.Encounter)
            .FirstOrDefaultAsync(v => v.Vitalsid == id);

        if (vital == null) return null;
        return VitalMapper.ToResponseDto(vital);
    }

    public async Task<IEnumerable<VitalResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var list = await _context.Vitals
            .Include(v => v.Patient)
            .Include(v => v.Encounter)
            .Where(v => v.Patientid == patientId)
            .OrderByDescending(v => v.Recordedat)
            .ToListAsync();

        return list.Select(VitalMapper.ToResponseDto);
    }

    public async Task<IEnumerable<VitalResponseDto>> GetByEncounterIdAsync(int encounterId)
    {
        var list = await _context.Vitals
            .Include(v => v.Patient)
            .Include(v => v.Encounter)
            .Where(v => v.Encounterid == encounterId)
            .OrderByDescending(v => v.Recordedat)
            .ToListAsync();

        return list.Select(VitalMapper.ToResponseDto);
    }

    public async Task<VitalResponseDto?> UpdateAsync(int id, UpdateVitalDto dto)
    {
        var entity = await _context.Vitals
            .Include(v => v.Patient)
            .Include(v => v.Encounter)
            .FirstOrDefaultAsync(v => v.Vitalsid == id);

        if (entity == null) return null;

        VitalMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return VitalMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Vitals.FindAsync(id);
        if (entity == null) return false;

        _context.Vitals.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}