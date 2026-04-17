// Services/SmartgoalService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class SmartgoalService : ISmartgoalService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidStatuses = new[]
    {
        "Active", "OnTrack", "Delayed", "Completed", "Cancelled"
    };

    private static readonly string[] ValidMeasurementTypes = new[]
    {
        "BP_SYSTOLIC", "BP_DIASTOLIC",
        "GLUCOSE", "HBA1C",
        "SPO2", "HEART_RATE",
        "WEIGHT", "BMI",
        "LDL",
        "DIET", "EXERCISE"
    };

    public SmartgoalService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Smartgoal> BaseQuery()
    {
        return _context.Smartgoals
            .Include(s => s.Patient)
            .Include(s => s.Careplan)
            .Include(s => s.Providerinfo);
    }

    public async Task<SmartgoalResponseDto> CreateAsync(CreateSmartgoalDto dto)
    {
        // Validate Careplan exists and is active
        var careplan = await _context.Careplans
            .FirstOrDefaultAsync(c => c.Careplanid == dto.Careplanid);
        if (careplan == null)
            throw new ArgumentException(
                $"Care plan with ID {dto.Careplanid} does not exist.");

        if (careplan.Status != "Active")
            throw new ArgumentException(
                "Goals can only be added to an active care plan.");

        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate careplan belongs to patient
        if (careplan.Patientid != dto.Patientid)
            throw new ArgumentException(
                "Care plan does not belong to the specified patient.");

        // Validate Provider exists if provided
        if (dto.Providerinfoid.HasValue)
        {
            var providerExists = await _context.Providerinfos
                .AnyAsync(p => p.Providerinfoid == dto.Providerinfoid);
            if (!providerExists)
                throw new ArgumentException(
                    $"Provider with ID {dto.Providerinfoid} does not exist.");
        }

        // Validate Measurement Type
        if (!ValidMeasurementTypes.Contains(
            dto.Measurementtype.ToUpper()))
            throw new ArgumentException(
                $"Invalid Measurement Type. Allowed: " +
                $"{string.Join(", ", ValidMeasurementTypes)}.");

        // Validate Status
        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        // Validate target date
        if (dto.Targetdate.HasValue && dto.Targetdate <= dto.Startdate)
            throw new ArgumentException(
                "Target date must be after start date.");

        // Validate progress
        if (dto.Targetvalue.HasValue && dto.Currentvalue.HasValue)
        {
            if (dto.Currentvalue < 0 || dto.Targetvalue < 0)
                throw new ArgumentException(
                    "Current value and target value cannot be negative.");
        }

        // Validate exercise minutes
        if (dto.Weeklyminutes.HasValue && dto.Weeklyminutes < 0)
            throw new ArgumentException(
                "Weekly minutes cannot be negative.");

        // Validate diet type for DIET measurement
        if (dto.Measurementtype.ToUpper() == "DIET" &&
            string.IsNullOrEmpty(dto.Diettype))
            throw new ArgumentException(
                "Diet type is required for DIET measurement goals.");

        // Validate exercise type for EXERCISE measurement
        if (dto.Measurementtype.ToUpper() == "EXERCISE" &&
            string.IsNullOrEmpty(dto.Exercisetype))
            throw new ArgumentException(
                "Exercise type is required for EXERCISE measurement goals.");

        var entity = SmartgoalMapper.ToEntity(dto);
        _context.Smartgoals.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(s => s.Smartgoalid == entity.Smartgoalid);

        return SmartgoalMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderByDescending(s => s.Createdat)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<SmartgoalResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(s => s.Smartgoalid == id);

        if (entity == null) return null;
        return SmartgoalMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(s => s.Patientid == patientId)
            .OrderByDescending(s => s.Createdat)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetActiveByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(s => s.Patientid == patientId &&
                        s.Status != "Completed" &&
                        s.Status != "Cancelled")
            .OrderBy(s => s.Targetdate)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetByCareplanIdAsync(
        long careplanId)
    {
        var list = await BaseQuery()
            .Where(s => s.Careplanid == careplanId)
            .OrderBy(s => s.Targetdate)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetByProviderIdAsync(
        long providerInfoId)
    {
        var list = await BaseQuery()
            .Where(s => s.Providerinfoid == providerInfoId)
            .OrderByDescending(s => s.Createdat)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetByStatusAsync(
        string status)
    {
        var list = await BaseQuery()
            .Where(s => s.Status != null &&
                        s.Status.ToLower() == status.ToLower())
            .OrderByDescending(s => s.Createdat)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetByMeasurementtypeAsync(
        string measurementtype)
    {
        var list = await BaseQuery()
            .Where(s => s.Measurementtype.ToUpper() ==
                        measurementtype.ToUpper())
            .OrderByDescending(s => s.Createdat)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetOverdueAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await BaseQuery()
            .Where(s => s.Targetdate.HasValue &&
                        s.Targetdate < today &&
                        s.Status != "Completed" &&
                        s.Status != "Cancelled")
            .OrderBy(s => s.Targetdate)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<IEnumerable<SmartgoalResponseDto>> GetAtRiskAsync()
    {
        var list = await BaseQuery()
            .Where(s => s.Status == "Active" &&
                        s.Progress < 40 &&
                        s.Status != "Completed" &&
                        s.Status != "Cancelled")
            .OrderBy(s => s.Progress)
            .ToListAsync();

        return list.Select(SmartgoalMapper.ToResponseDto);
    }

    public async Task<SmartgoalResponseDto?> UpdateProgressAsync(
        long id, SmartgoalProgressUpdateDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(s => s.Smartgoalid == id);

        if (entity == null) return null;

        if (entity.Status == "Completed")
            throw new ArgumentException(
                "Cannot update progress on a completed goal.");

        if (entity.Status == "Cancelled")
            throw new ArgumentException(
                "Cannot update progress on a cancelled goal.");

        if (dto.Currentvalue < 0)
            throw new ArgumentException(
                "Current value cannot be negative.");

        SmartgoalMapper.UpdateProgress(entity, dto);
        await _context.SaveChangesAsync();
        return SmartgoalMapper.ToResponseDto(entity);
    }

    public async Task<SmartgoalResponseDto?> UpdateAsync(
        long id, UpdateSmartgoalDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(s => s.Smartgoalid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        if (dto.Targetdate.HasValue && dto.Targetdate <= entity.Startdate)
            throw new ArgumentException(
                "Target date must be after start date.");

        if (dto.Weeklyminutes.HasValue && dto.Weeklyminutes < 0)
            throw new ArgumentException(
                "Weekly minutes cannot be negative.");

        SmartgoalMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return SmartgoalMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Smartgoals.FindAsync(id);
        if (entity == null) return false;

        _context.Smartgoals.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}