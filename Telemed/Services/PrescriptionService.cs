// Services/PrescriptionService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly TelemedDbContext _context;

    public PrescriptionService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<PrescriptionResponseDto> CreateAsync(CreatePrescriptionDto dto)
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

        // Validate Provider exists
        var providerExists = await _context.Providers
            .AnyAsync(p => p.Providerid == dto.Providerid);
        if (!providerExists)
            throw new ArgumentException($"Provider with ID {dto.Providerid} does not exist.");

        var entity = PrescriptionMapper.ToEntity(dto);
        _context.Prescriptions.Add(entity);
        await _context.SaveChangesAsync();

        // Reload with related data for full response
        var created = await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Encounter)
            .FirstOrDefaultAsync(p => p.Prescriptionid == entity.Prescriptionid);

        return PrescriptionMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<PrescriptionResponseDto>> GetAllAsync()
    {
        var list = await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Encounter)
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PrescriptionMapper.ToResponseDto);
    }

    public async Task<PrescriptionResponseDto?> GetByIdAsync(int id)
    {
        var prescription = await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Encounter)
            .FirstOrDefaultAsync(p => p.Prescriptionid == id);

        if (prescription == null) return null;
        return PrescriptionMapper.ToResponseDto(prescription);
    }

    public async Task<IEnumerable<PrescriptionResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var list = await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Encounter)
            .Where(p => p.Patientid == patientId)
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PrescriptionMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PrescriptionResponseDto>> GetByEncounterIdAsync(int encounterId)
    {
        var list = await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Encounter)
            .Where(p => p.Encounterid == encounterId)
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PrescriptionMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PrescriptionResponseDto>> GetByProviderIdAsync(int providerId)
    {
        var list = await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Encounter)
            .Where(p => p.Providerid == providerId)
            .OrderByDescending(p => p.Createdat)
            .ToListAsync();

        return list.Select(PrescriptionMapper.ToResponseDto);
    }

    public async Task<PrescriptionResponseDto?> UpdateAsync(int id, UpdatePrescriptionDto dto)
    {
        var entity = await _context.Prescriptions
            .Include(p => p.Patient)
            .Include(p => p.Provider)
            .Include(p => p.Encounter)
            .FirstOrDefaultAsync(p => p.Prescriptionid == id);

        if (entity == null) return null;

        PrescriptionMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return PrescriptionMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Prescriptions.FindAsync(id);
        if (entity == null) return false;

        _context.Prescriptions.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}