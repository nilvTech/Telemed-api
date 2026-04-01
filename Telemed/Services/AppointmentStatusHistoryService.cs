// Services/AppointmentStatusHistoryService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class AppointmentStatusHistoryService : IAppointmentStatusHistoryService
{
    private readonly TelemedDbContext _context;

    public AppointmentStatusHistoryService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentStatusHistoryResponseDto> CreateAsync(
        CreateAppointmentStatusHistoryDto dto)
    {
        // Validate Appointment exists
        var appointmentExists = await _context.Appointments
            .AnyAsync(a => a.Appointmentid == dto.Appointmentid);
        if (!appointmentExists)
            throw new ArgumentException(
                $"Appointment with ID {dto.Appointmentid} does not exist.");

        // Validate Status
        var validStatuses = new[]
        {
            "Scheduled", "Confirmed", "CheckedIn",
            "InProgress", "Completed", "Cancelled",
            "NoShow", "Rescheduled"
        };
        if (!validStatuses.Contains(dto.Status,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                "Invalid Status. Allowed: Scheduled, Confirmed, CheckedIn, " +
                "InProgress, Completed, Cancelled, NoShow, Rescheduled.");

        var entity = AppointmentStatusHistoryMapper.ToEntity(dto);
        _context.Appointmentstatushistories.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.Appointmentstatushistories
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Provider)
            .FirstOrDefaultAsync(h => h.Id == entity.Id);

        return AppointmentStatusHistoryMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<AppointmentStatusHistoryResponseDto>> GetAllAsync()
    {
        var list = await _context.Appointmentstatushistories
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Provider)
            .OrderByDescending(h => h.Changedat)
            .ToListAsync();

        return list.Select(AppointmentStatusHistoryMapper.ToResponseDto);
    }

    public async Task<AppointmentStatusHistoryResponseDto?> GetByIdAsync(long id)
    {
        var history = await _context.Appointmentstatushistories
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Provider)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (history == null) return null;
        return AppointmentStatusHistoryMapper.ToResponseDto(history);
    }

    public async Task<IEnumerable<AppointmentStatusHistoryResponseDto>> GetByAppointmentIdAsync(
        long appointmentId)
    {
        var list = await _context.Appointmentstatushistories
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Provider)
            .Where(h => h.Appointmentid == appointmentId)
            .OrderByDescending(h => h.Changedat)
            .ToListAsync();

        return list.Select(AppointmentStatusHistoryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentStatusHistoryResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Appointmentstatushistories
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Provider)
            .Where(h => h.Appointment != null &&
                        h.Appointment.Patientid == patientId)
            .OrderByDescending(h => h.Changedat)
            .ToListAsync();

        return list.Select(AppointmentStatusHistoryMapper.ToResponseDto);
    }

    public async Task<AppointmentStatusHistoryResponseDto?> GetLatestByAppointmentIdAsync(
        long appointmentId)
    {
        var history = await _context.Appointmentstatushistories
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(h => h.Appointment)
                .ThenInclude(a => a!.Provider)
            .Where(h => h.Appointmentid == appointmentId)
            .OrderByDescending(h => h.Changedat)
            .FirstOrDefaultAsync();

        if (history == null) return null;
        return AppointmentStatusHistoryMapper.ToResponseDto(history);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Appointmentstatushistories.FindAsync(id);
        if (entity == null) return false;

        _context.Appointmentstatushistories.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}