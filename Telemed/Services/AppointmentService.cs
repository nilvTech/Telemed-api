using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class AppointmentService : IAppointmentService
{
    private readonly TelemedDbContext _context;

    public AppointmentService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto)
    {
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

        var entity = AppointmentMapper.ToEntity(dto);
        _context.Appointments.Add(entity);
        await _context.SaveChangesAsync();

        // Reload with related data for full response
        var created = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .FirstOrDefaultAsync(a => a.Appointmentid == entity.Appointmentid);

        return AppointmentMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetAllAsync()
    {
        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .OrderByDescending(a => a.Scheduleddatetime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(int id)
    {
        var appt = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .FirstOrDefaultAsync(a => a.Appointmentid == id);

        if (appt == null) return null;
        return AppointmentMapper.ToResponseDto(appt);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Patientid == patientId)
            .OrderByDescending(a => a.Scheduleddatetime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByProviderIdAsync(int providerId)
    {
        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Providerid == providerId)
            .OrderByDescending(a => a.Scheduleddatetime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<AppointmentResponseDto?> UpdateAsync(int id, UpdateAppointmentDto dto)
    {
        var entity = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .FirstOrDefaultAsync(a => a.Appointmentid == id);

        if (entity == null) return null;

        AppointmentMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return AppointmentMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Appointments.FindAsync(id);
        if (entity == null) return false;

        _context.Appointments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}