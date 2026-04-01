// Services/AppointmentService.cs
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
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate Provider exists
        var providerExists = await _context.Providers
            .AnyAsync(p => p.Providerid == dto.Providerid);
        if (!providerExists)
            throw new ArgumentException(
                $"Provider with ID {dto.Providerid} does not exist.");

        // Validate VisitType
        var validVisitTypes = new[]
        {
            "InPerson", "Telemedicine", "Phone", "HomeVisit"
        };
        if (!validVisitTypes.Contains(dto.Visittype,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                "Invalid VisitType. Allowed: InPerson, Telemedicine, Phone, HomeVisit.");

        // Validate VisitMode
        var validVisitModes = new[]
        {
            "Scheduled", "WalkIn", "Emergency", "Urgent"
        };
        if (!validVisitModes.Contains(dto.Visitmode,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                "Invalid VisitMode. Allowed: Scheduled, WalkIn, Emergency, Urgent.");

        // Validate Priority
        if (!string.IsNullOrEmpty(dto.Priority))
        {
            var validPriorities = new[] { "Normal", "Urgent", "Emergency" };
            if (!validPriorities.Contains(dto.Priority,
                StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(
                    "Invalid Priority. Allowed: Normal, Urgent, Emergency.");
        }

        // Check provider slot conflict
        var conflictExists = await _context.Appointments
            .AnyAsync(a =>
                a.Providerid == dto.Providerid &&
                a.Appointmentdate == dto.Appointmentdate &&
                a.Starttime == dto.Starttime &&
                a.Isactive == true &&
                a.Status != "Cancelled");
        if (conflictExists)
            throw new ArgumentException(
                "Provider already has an appointment at this date and time.");

        var entity = AppointmentMapper.ToEntity(dto);
        _context.Appointments.Add(entity);
        await _context.SaveChangesAsync();

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
            .Where(a => a.Isactive == true)
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<AppointmentResponseDto?> GetByIdAsync(long id)
    {
        var appt = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .FirstOrDefaultAsync(a => a.Appointmentid == id);

        if (appt == null) return null;
        return AppointmentMapper.ToResponseDto(appt);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Patientid == patientId && a.Isactive == true)
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByProviderIdAsync(
        long providerId)
    {
        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Providerid == providerId && a.Isactive == true)
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByDateAsync(
        DateOnly date)
    {
        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Appointmentdate == date && a.Isactive == true)
            .OrderBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByStatusAsync(
        string status)
    {
        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Status.ToLower() == status.ToLower()
                     && a.Isactive == true)
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetTodayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Appointmentdate == today
                     && a.Isactive == true
                     && a.Status != "Cancelled")
            .OrderBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetUpcomingByPatientAsync(
        long patientId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Patientid == patientId
                     && a.Appointmentdate >= today
                     && a.Isactive == true
                     && a.Status != "Cancelled"
                     && a.Status != "Completed")
            .OrderBy(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetUpcomingByProviderAsync(
        long providerId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Providerid == providerId
                     && a.Appointmentdate >= today
                     && a.Isactive == true
                     && a.Status != "Cancelled"
                     && a.Status != "Completed")
            .OrderBy(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<AppointmentResponseDto?> UpdateAsync(
        long id, UpdateAppointmentDto dto)
    {
        var entity = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .FirstOrDefaultAsync(a => a.Appointmentid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status))
        {
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
        }

        AppointmentMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return AppointmentMapper.ToResponseDto(entity);
    }

    public async Task<AppointmentResponseDto?> UpdateStatusAsync(
        long id, AppointmentStatusUpdateDto dto)
    {
        var entity = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .FirstOrDefaultAsync(a => a.Appointmentid == id);

        if (entity == null) return null;

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

        entity.Status = dto.Status;
        entity.Updatedby = dto.Updatedby;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return AppointmentMapper.ToResponseDto(entity);
    }

    public async Task<AppointmentResponseDto?> CheckInAsync(
        long id, CheckInDto dto)
    {
        var entity = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .FirstOrDefaultAsync(a => a.Appointmentid == id);

        if (entity == null) return null;

        entity.Status = "CheckedIn";
        entity.Checkedintime = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);
        entity.Tokennumber = dto.Tokennumber;
        entity.Queueposition = dto.Queueposition;
        entity.Updatedby = dto.Updatedby;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return AppointmentMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetFollowUpsAsync(
        long patientId)
    {
        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Provider)
            .Where(a => a.Patientid == patientId
                     && a.Isfollowup == true
                     && a.Isactive == true)
            .OrderByDescending(a => a.Appointmentdate)
            .ToListAsync();

        return list.Select(AppointmentMapper.ToResponseDto);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Appointments.FindAsync(id);
        if (entity == null) return false;

        // Soft delete only — US healthcare never hard deletes
        entity.Isactive = false;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return true;
    }
}