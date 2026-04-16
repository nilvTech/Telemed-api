// Services/RpmmonitoringService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class RpmmonitoringService : IRpmmonitoringService
{
    private readonly TelemedDbContext _context;

    public RpmmonitoringService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Rpmmonitoring> BaseQuery()
    {
        return _context.Rpmmonitorings
            .Include(r => r.Patient)
            .Include(r => r.ReviewedByProvider);
    }

    public async Task<RpmmonitoringResponseDto> CreateAsync(CreateRpmmonitoringDto dto)
    {
        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException($"Patient with ID {dto.Patientid} does not exist.");

        //  Validate SourceData
        var allowedSourceData = new[] { "Manual", "Bluetooth", "CellularHub", "WiFi", "App" };

        if (!string.IsNullOrEmpty(dto.Sourcedata) &&
            !allowedSourceData.Contains(dto.Sourcedata))
        {
            throw new ArgumentException($"Invalid Sourcedata '{dto.Sourcedata}'. Allowed values: {string.Join(", ", allowedSourceData)}");
        }

        // Validate at least one reading provided
        if (!dto.Systolic.HasValue && !dto.Diastolic.HasValue &&
            !dto.Heartrate.HasValue && !dto.Spo2.HasValue &&
            !dto.Glucose.HasValue && !dto.Temperature.HasValue &&
            !dto.Weight.HasValue && !dto.Respiratoryrate.HasValue)
            throw new ArgumentException("At least one vital reading must be provided.");

        // Validate BP range
        if (dto.Systolic.HasValue &&
            (dto.Systolic < 50 || dto.Systolic > 300))
            throw new ArgumentException("Systolic BP must be between 50 and 300.");

        if (dto.Diastolic.HasValue &&
            (dto.Diastolic < 30 || dto.Diastolic > 200))
            throw new ArgumentException("Diastolic BP must be between 30 and 200.");

        // Validate SpO2
        if (dto.Spo2.HasValue && (dto.Spo2 < 50 || dto.Spo2 > 100))
            throw new ArgumentException("SpO2 must be between 50 and 100.");

        // Validate Heart Rate
        if (dto.Heartrate.HasValue &&
            (dto.Heartrate < 20 || dto.Heartrate > 300))
            throw new ArgumentException("Heart rate must be between 20 and 300.");

        var entity = RpmmonitoringMapper.ToEntity(dto);
        _context.Rpmmonitorings.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(r => r.Rpmmonitoringid == entity.Rpmmonitoringid);

        return RpmmonitoringMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<RpmmonitoringResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderByDescending(r => r.Readingdate)
            .ToListAsync();

        return list.Select(RpmmonitoringMapper.ToResponseDto);
    }

    public async Task<RpmmonitoringResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(r => r.Rpmmonitoringid == id);

        if (entity == null) return null;
        return RpmmonitoringMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<RpmmonitoringResponseDto>> GetByPatientIdAsync(long patientId)
    {
        var list = await BaseQuery()
            .Where(r => r.Patientid == patientId)
            .OrderByDescending(r => r.Readingdate)
            .ToListAsync();

        return list.Select(RpmmonitoringMapper.ToResponseDto);
    }

    public async Task<IEnumerable<RpmmonitoringResponseDto>> GetByPatientIdAndDateRangeAsync(
        long patientId, DateTime from, DateTime to)
    {
        var list = await BaseQuery()
            .Where(r => r.Patientid == patientId &&
                        r.Readingdate >= from &&
                        r.Readingdate <= to)
            .OrderByDescending(r => r.Readingdate)
            .ToListAsync();

        return list.Select(RpmmonitoringMapper.ToResponseDto);
    }

    public async Task<IEnumerable<RpmmonitoringResponseDto>> GetUnreviewedAsync()
    {
        var list = await BaseQuery()
            .Where(r => r.Isreviewed == false)
            .OrderByDescending(r => r.Readingdate)
            .ToListAsync();

        return list.Select(RpmmonitoringMapper.ToResponseDto);
    }

    public async Task<IEnumerable<RpmmonitoringResponseDto>> GetUnreviewedByPatientAsync(long patientId)
    {
        var list = await BaseQuery()
            .Where(r => r.Patientid == patientId && r.Isreviewed == false)
            .OrderByDescending(r => r.Readingdate)
            .ToListAsync();

        return list.Select(RpmmonitoringMapper.ToResponseDto);
    }

    public async Task<RpmmonitoringResponseDto?> GetLatestByPatientAsync(long patientId)
    {
        var entity = await BaseQuery()
            .Where(r => r.Patientid == patientId)
            .OrderByDescending(r => r.Readingdate)
            .FirstOrDefaultAsync();

        if (entity == null) return null;
        return RpmmonitoringMapper.ToResponseDto(entity);
    }

    public async Task<RpmmonitoringResponseDto?> UpdateAsync(long id, UpdateRpmmonitoringDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(r => r.Rpmmonitoringid == id);

        if (entity == null) return null;

        if (entity.Isreviewed == true)
            throw new ArgumentException("Cannot update a reading that has already been reviewed.");

        RpmmonitoringMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return RpmmonitoringMapper.ToResponseDto(entity);
    }

    public async Task<RpmmonitoringResponseDto?> MarkReviewedAsync(long id, RpmReviewDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(r => r.Rpmmonitoringid == id);

        if (entity == null) return null;

        // Validate provider exists
        var providerExists = await _context.Providerinfos
            .AnyAsync(p => p.Providerinfoid == dto.Reviewedby);
        if (!providerExists)
            throw new ArgumentException($"Provider with ID {dto.Reviewedby} does not exist.");

        entity.Isreviewed = true;
        entity.Reviewedby = dto.Reviewedby;
        entity.Reviewedat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        entity.Updatedat = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        if (!string.IsNullOrEmpty(dto.Note))
            entity.Note = dto.Note;

        await _context.SaveChangesAsync();

        // Reload with updated provider info
        var updated = await BaseQuery()
            .FirstOrDefaultAsync(r => r.Rpmmonitoringid == id);

        return RpmmonitoringMapper.ToResponseDto(updated!);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Rpmmonitorings.FindAsync(id);
        if (entity == null) return false;

        _context.Rpmmonitorings.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}