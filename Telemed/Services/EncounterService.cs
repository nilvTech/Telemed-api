using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class EncounterService : IEncounterService
{
    private readonly TelemedDbContext _context;

    public EncounterService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<EncounterResponseDto> CreateAsync(CreateEncounterDto dto)
    {
        // Validate Appointment exists
        var appointmentExists = await _context.Appointments
            .AnyAsync(a => a.Appointmentid == dto.Appointmentid);
        if (!appointmentExists)
            throw new ArgumentException($"Appointment with ID {dto.Appointmentid} does not exist.");

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

        var entity = EncounterMapper.ToEntity(dto);
        _context.Encounters.Add(entity);
        await _context.SaveChangesAsync();

        // Reload with related data for full response
        var created = await _context.Encounters
            .Include(e => e.Patient)
            .Include(e => e.Provider)
            .Include(e => e.Appointment)
            .FirstOrDefaultAsync(e => e.Encounterid == entity.Encounterid);

        return EncounterMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<EncounterResponseDto>> GetAllAsync()
    {
        var list = await _context.Encounters
            .Include(e => e.Patient)
            .Include(e => e.Provider)
            .Include(e => e.Appointment)
            .OrderByDescending(e => e.Encounterdate)
            .ToListAsync();

        return list.Select(EncounterMapper.ToResponseDto);
    }

    public async Task<EncounterResponseDto?> GetByIdAsync(int id)
    {
        var encounter = await _context.Encounters
            .Include(e => e.Patient)
            .Include(e => e.Provider)
            .Include(e => e.Appointment)
            .FirstOrDefaultAsync(e => e.Encounterid == id);

        if (encounter == null) return null;
        return EncounterMapper.ToResponseDto(encounter);
    }

    public async Task<IEnumerable<EncounterResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var list = await _context.Encounters
            .Include(e => e.Patient)
            .Include(e => e.Provider)
            .Include(e => e.Appointment)
            .Where(e => e.Patientid == patientId)
            .OrderByDescending(e => e.Encounterdate)
            .ToListAsync();

        return list.Select(EncounterMapper.ToResponseDto);
    }

    public async Task<IEnumerable<EncounterResponseDto>> GetByAppointmentIdAsync(int appointmentId)
    {
        var list = await _context.Encounters
            .Include(e => e.Patient)
            .Include(e => e.Provider)
            .Include(e => e.Appointment)
            .Where(e => e.Appointmentid == appointmentId)
            .OrderByDescending(e => e.Encounterdate)
            .ToListAsync();

        return list.Select(EncounterMapper.ToResponseDto);
    }

    public async Task<IEnumerable<EncounterResponseDto>> GetByProviderIdAsync(int providerId)
    {
        var list = await _context.Encounters
            .Include(e => e.Patient)
            .Include(e => e.Provider)
            .Include(e => e.Appointment)
            .Where(e => e.Providerid == providerId)
            .OrderByDescending(e => e.Encounterdate)
            .ToListAsync();

        return list.Select(EncounterMapper.ToResponseDto);
    }

    public async Task<EncounterResponseDto?> UpdateAsync(int id, UpdateEncounterDto dto)
    {
        var entity = await _context.Encounters
            .Include(e => e.Patient)
            .Include(e => e.Provider)
            .Include(e => e.Appointment)
            .FirstOrDefaultAsync(e => e.Encounterid == id);

        if (entity == null) return null;

        EncounterMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return EncounterMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Encounters.FindAsync(id);
        if (entity == null) return false;

        _context.Encounters.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}