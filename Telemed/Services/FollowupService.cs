// Services/FollowupService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class FollowupService : IFollowupService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidStatuses = new[]
    {
        "Scheduled", "Completed", "Cancelled", "NoShow"
    };

    private static readonly string[] ValidFollowupTypes = new[]
    {
        "Phone Call", "Visit", "Lab Review",
        "Telehealth", "RPM Review", "Medication Review"
    };

    public FollowupService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Followup> BaseQuery()
    {
        return _context.Followups
            .Include(f => f.Patient)
            .Include(f => f.Appointment);
    }

    public async Task<FollowupResponseDto> CreateAsync(CreateFollowupDto dto)
    {
        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate Appointment if provided
        if (dto.Appointmentid.HasValue)
        {
            var appointmentExists = await _context.Appointments
                .AnyAsync(a => a.Appointmentid == dto.Appointmentid);
            if (!appointmentExists)
                throw new ArgumentException(
                    $"Appointment with ID {dto.Appointmentid} does not exist.");
        }

        // Validate followup type
        if (!ValidFollowupTypes.Contains(dto.Followuptype,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Follow-up Type. Allowed: " +
                $"{string.Join(", ", ValidFollowupTypes)}.");

        // Validate status
        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        // Validate followup date not in past
        if (dto.Followupdate < DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException(
                "Follow-up date cannot be in the past.");

        var entity = FollowupMapper.ToEntity(dto);
        _context.Followups.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(f => f.Followupid == entity.Followupid);

        return FollowupMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<FollowupResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(FollowupMapper.ToResponseDto);
    }

    public async Task<FollowupResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(f => f.Followupid == id);

        if (entity == null) return null;
        return FollowupMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<FollowupResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(f => f.Patientid == patientId)
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(FollowupMapper.ToResponseDto);
    }

    public async Task<IEnumerable<FollowupResponseDto>> GetByAppointmentIdAsync(
        long appointmentId)
    {
        var list = await BaseQuery()
            .Where(f => f.Appointmentid == appointmentId)
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(FollowupMapper.ToResponseDto);
    }

    public async Task<IEnumerable<FollowupResponseDto>> GetByStatusAsync(
        string status)
    {
        var list = await BaseQuery()
            .Where(f => f.Status != null &&
                        f.Status.ToLower() == status.ToLower())
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(FollowupMapper.ToResponseDto);
    }

    public async Task<IEnumerable<FollowupResponseDto>> GetByTypeAsync(
        string followuptype)
    {
        var list = await BaseQuery()
            .Where(f => f.Followuptype.ToLower() ==
                        followuptype.ToLower())
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(FollowupMapper.ToResponseDto);
    }

    public async Task<IEnumerable<FollowupResponseDto>> GetOverdueAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await BaseQuery()
            .Where(f => f.Followupdate < today &&
                        f.Status == "Scheduled")
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(FollowupMapper.ToResponseDto);
    }

    public async Task<IEnumerable<FollowupResponseDto>> GetTodayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await BaseQuery()
            .Where(f => f.Followupdate == today &&
                        f.Status == "Scheduled")
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(FollowupMapper.ToResponseDto);
    }

    public async Task<IEnumerable<FollowupResponseDto>> GetUpcomingAsync(int days)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var endDate = today.AddDays(days);

        var list = await BaseQuery()
            .Where(f => f.Followupdate >= today &&
                        f.Followupdate <= endDate &&
                        f.Status == "Scheduled")
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(FollowupMapper.ToResponseDto);
    }

    public async Task<FollowupResponseDto?> UpdateAsync(
        long id, UpdateFollowupDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(f => f.Followupid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        if (!string.IsNullOrEmpty(dto.Followuptype) &&
            !ValidFollowupTypes.Contains(dto.Followuptype,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Follow-up Type. Allowed: " +
                $"{string.Join(", ", ValidFollowupTypes)}.");

        FollowupMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return FollowupMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Followups.FindAsync(id);
        if (entity == null) return false;

        _context.Followups.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}