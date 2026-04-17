// Services/CareplanService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class CareplanService : ICareplanService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidStatuses = new[]
    {
        "Active", "Completed", "OnHold", "Discontinued"
    };

    private static readonly string[] ValidRiskLevels = new[]
    {
        "Low", "Medium", "High", "Very High"
    };

    private static readonly string[] ValidReviewFrequencies = new[]
    {
        "Weekly", "BiWeekly", "Monthly",
        "BiMonthly", "Quarterly", "SemiAnnual", "Annual"
    };

    public CareplanService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Careplan> BaseQuery()
    {
        return _context.Careplans
            .Include(c => c.Patient)
            .Include(c => c.Providerinfo)
                .ThenInclude(p => p!.Providerprofile);
    }

    public async Task<CareplanResponseDto> CreateAsync(CreateCareplanDto dto)
    {
        // Validate Patient exists
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.Patientid == dto.Patientid);
        if (patient == null)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate Provider exists if provided
        if (dto.Providerinfoid.HasValue)
        {
            var providerExists = await _context.Providerinfos
                .AnyAsync(p => p.Providerinfoid == dto.Providerinfoid);
            if (!providerExists)
                throw new ArgumentException(
                    $"Provider with ID {dto.Providerinfoid} does not exist.");
        }

        // Validate no active careplan for same patient
        var activeExists = await _context.Careplans
            .AnyAsync(c => c.Patientid == dto.Patientid &&
                           c.Status == "Active");
        if (activeExists)
            throw new ArgumentException(
                "Patient already has an active care plan. " +
                "Please complete or discontinue it before creating a new one.");

        // Validate Status
        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        // Validate Risk Level
        if (!string.IsNullOrEmpty(dto.Risklevel) &&
            !ValidRiskLevels.Contains(dto.Risklevel, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Risk Level. Allowed: {string.Join(", ", ValidRiskLevels)}.");

        // Validate date range
        if (dto.Enddate.HasValue && dto.Enddate <= dto.Startdate)
            throw new ArgumentException(
                "End date must be after start date.");

        // Validate BP targets
        if (dto.Bpsystolictarget.HasValue &&
            (dto.Bpsystolictarget < 70 || dto.Bpsystolictarget > 200))
            throw new ArgumentException(
                "Systolic target must be between 70 and 200 mmHg.");

        if (dto.Bpdiastolictarget.HasValue &&
            (dto.Bpdiastolictarget < 40 || dto.Bpdiastolictarget > 130))
            throw new ArgumentException(
                "Diastolic target must be between 40 and 130 mmHg.");

        // Validate glucose range
        if (dto.Glucosetargetmin.HasValue && dto.Glucosetargetmax.HasValue &&
            dto.Glucosetargetmin >= dto.Glucosetargetmax)
            throw new ArgumentException(
                "Glucose target min must be less than max.");

        // Validate heart rate range
        if (dto.Heartratetargetmin.HasValue && dto.Heartratetargetmax.HasValue &&
            dto.Heartratetargetmin >= dto.Heartratetargetmax)
            throw new ArgumentException(
                "Heart rate target min must be less than max.");

        // Validate HbA1c target (ADA standard 6.5-8.0%)
        if (dto.Hba1ctarget.HasValue &&
            (dto.Hba1ctarget < 5.0m || dto.Hba1ctarget > 15.0m))
            throw new ArgumentException(
                "HbA1c target must be between 5.0 and 15.0%.");

        // Validate review frequency
        if (!string.IsNullOrEmpty(dto.Reviewfrequency) &&
            !ValidReviewFrequencies.Contains(dto.Reviewfrequency,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Review Frequency. Allowed: " +
                $"{string.Join(", ", ValidReviewFrequencies)}.");

        var entity = CareplanMapper.ToEntity(dto);
        _context.Careplans.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careplanid == entity.Careplanid);

        return CareplanMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<CareplanResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(CareplanMapper.ToResponseDto);
    }

    public async Task<CareplanResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careplanid == id);

        if (entity == null) return null;
        return CareplanMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<CareplanResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(c => c.Patientid == patientId)
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(CareplanMapper.ToResponseDto);
    }

    public async Task<CareplanResponseDto?> GetActiveByPatientIdAsync(
        long patientId)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Patientid == patientId &&
                                      c.Status == "Active");

        if (entity == null) return null;
        return CareplanMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<CareplanResponseDto>> GetByProviderIdAsync(
        long providerInfoId)
    {
        var list = await BaseQuery()
            .Where(c => c.Providerinfoid == providerInfoId)
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(CareplanMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareplanResponseDto>> GetByStatusAsync(
        string status)
    {
        var list = await BaseQuery()
            .Where(c => c.Status != null &&
                        c.Status.ToLower() == status.ToLower())
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(CareplanMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareplanResponseDto>> GetByRisklevelAsync(
        string risklevel)
    {
        var list = await BaseQuery()
            .Where(c => c.Risklevel != null &&
                        c.Risklevel.ToLower() == risklevel.ToLower() &&
                        c.Status == "Active")
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(CareplanMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareplanResponseDto>> GetOverdueForReviewAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await BaseQuery()
            .Where(c => c.Nextreviewdate.HasValue &&
                        c.Nextreviewdate < today &&
                        c.Status == "Active")
            .OrderBy(c => c.Nextreviewdate)
            .ToListAsync();

        return list.Select(CareplanMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareplanResponseDto>> GetDueForReviewTodayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await BaseQuery()
            .Where(c => c.Nextreviewdate == today &&
                        c.Status == "Active")
            .OrderBy(c => c.Patientid)
            .ToListAsync();

        return list.Select(CareplanMapper.ToResponseDto);
    }

    // CCM — Plans that haven't met the 20-minute monthly threshold
    public async Task<IEnumerable<CareplanResponseDto>> GetCcmNotMetAsync()
    {
        var list = await BaseQuery()
            .Where(c => c.Status == "Active" &&
                        (c.Ccmminutes == null || c.Ccmminutes < 20))
            .OrderBy(c => c.Ccmminutes)
            .ToListAsync();

        return list.Select(CareplanMapper.ToResponseDto);
    }

    // Add CCM minutes to existing plan
    public async Task<CareplanResponseDto?> AddCcmMinutesAsync(
        long id, int minutes, long? updatedby)
    {
        if (minutes <= 0)
            throw new ArgumentException(
                "CCM minutes must be greater than 0.");

        if (minutes > 120)
            throw new ArgumentException(
                "Cannot add more than 120 minutes in a single entry.");

        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careplanid == id);

        if (entity == null) return null;

        if (entity.Status != "Active")
            throw new ArgumentException(
                "CCM minutes can only be added to an active care plan.");

        entity.Ccmminutes = (entity.Ccmminutes ?? 0) + minutes;
        entity.Updatedby = updatedby;
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return CareplanMapper.ToResponseDto(entity);
    }

    public async Task<CareplanResponseDto?> UpdateAsync(
        long id, UpdateCareplanDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careplanid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        if (!string.IsNullOrEmpty(dto.Risklevel) &&
            !ValidRiskLevels.Contains(dto.Risklevel, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Risk Level. Allowed: {string.Join(", ", ValidRiskLevels)}.");

        if (dto.Enddate.HasValue && dto.Enddate <= entity.Startdate)
            throw new ArgumentException(
                "End date must be after start date.");

        if (dto.Glucosetargetmin.HasValue && dto.Glucosetargetmax.HasValue &&
            dto.Glucosetargetmin >= dto.Glucosetargetmax)
            throw new ArgumentException(
                "Glucose target min must be less than max.");

        if (dto.Heartratetargetmin.HasValue && dto.Heartratetargetmax.HasValue &&
            dto.Heartratetargetmin >= dto.Heartratetargetmax)
            throw new ArgumentException(
                "Heart rate target min must be less than max.");

        CareplanMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return CareplanMapper.ToResponseDto(entity);
    }

    public async Task<CareplanResponseDto?> UpdateStatusAsync(
        long id, CareplanStatusUpdateDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careplanid == id);

        if (entity == null) return null;

        if (!ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        entity.Status = dto.Status;
        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        // Auto set end date when completing or discontinuing
        if (dto.Status is "Completed" or "Discontinued" &&
            !entity.Enddate.HasValue)
            entity.Enddate = DateOnly.FromDateTime(DateTime.Today);

        await _context.SaveChangesAsync();
        return CareplanMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Careplans.FindAsync(id);
        if (entity == null) return false;

        _context.Careplans.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}