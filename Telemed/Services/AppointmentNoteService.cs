// Services/AppointmentNoteService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class AppointmentNoteService : IAppointmentNoteService
{
    private readonly TelemedDbContext _context;

    public AppointmentNoteService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentNoteResponseDto> CreateAsync(
        CreateAppointmentNoteDto dto)
    {
        // Validate Appointment exists
        var appointmentExists = await _context.Appointments
            .AnyAsync(a => a.Appointmentid == dto.Appointmentid);
        if (!appointmentExists)
            throw new ArgumentException(
                $"Appointment with ID {dto.Appointmentid} does not exist.");

        // Validate Notes not empty
        if (string.IsNullOrWhiteSpace(dto.Notes))
            throw new ArgumentException("Notes cannot be empty.");

        var entity = AppointmentNoteMapper.ToEntity(dto);
        _context.Appointmentnotes.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.Appointmentnotes
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Provider)
            .FirstOrDefaultAsync(n => n.Id == entity.Id);

        return AppointmentNoteMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<AppointmentNoteResponseDto>> GetAllAsync()
    {
        var list = await _context.Appointmentnotes
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Provider)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(AppointmentNoteMapper.ToResponseDto);
    }

    public async Task<AppointmentNoteResponseDto?> GetByIdAsync(long id)
    {
        var note = await _context.Appointmentnotes
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Provider)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (note == null) return null;
        return AppointmentNoteMapper.ToResponseDto(note);
    }

    public async Task<IEnumerable<AppointmentNoteResponseDto>> GetByAppointmentIdAsync(
        long appointmentId)
    {
        var list = await _context.Appointmentnotes
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Provider)
            .Where(n => n.Appointmentid == appointmentId)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(AppointmentNoteMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentNoteResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Appointmentnotes
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Provider)
            .Where(n => n.Appointment != null &&
                        n.Appointment.Patientid == patientId)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(AppointmentNoteMapper.ToResponseDto);
    }

    public async Task<AppointmentNoteResponseDto?> UpdateAsync(
        long id, UpdateAppointmentNoteDto dto)
    {
        var entity = await _context.Appointmentnotes
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(n => n.Appointment)
                .ThenInclude(a => a!.Provider)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (entity == null) return null;

        if (string.IsNullOrWhiteSpace(dto.Notes))
            throw new ArgumentException("Notes cannot be empty.");

        AppointmentNoteMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return AppointmentNoteMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Appointmentnotes.FindAsync(id);
        if (entity == null) return false;

        _context.Appointmentnotes.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}