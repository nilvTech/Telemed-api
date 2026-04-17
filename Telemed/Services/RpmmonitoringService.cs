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
        // Validate Patient
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException($"Patient with ID {dto.Patientid} does not exist.");

        // Validate SourceData
        var allowedSourceData = new[] { "Manual", "Bluetooth", "CellularHub", "WiFi", "App" };
        if (!string.IsNullOrEmpty(dto.Sourcedata) &&
            !allowedSourceData.Contains(dto.Sourcedata))
        {
            throw new ArgumentException($"Invalid Sourcedata. Allowed: {string.Join(", ", allowedSourceData)}");
        }

        // Validate at least one vital
        if (!dto.Systolic.HasValue && !dto.Diastolic.HasValue &&
            !dto.Heartrate.HasValue && !dto.Spo2.HasValue &&
            !dto.Glucose.HasValue && !dto.Temperature.HasValue &&
            !dto.Weight.HasValue && !dto.Respiratoryrate.HasValue)
        {
            throw new ArgumentException("At least one vital reading must be provided.");
        }

        var entity = RpmmonitoringMapper.ToEntity(dto);

        _context.Rpmmonitorings.Add(entity);
        await _context.SaveChangesAsync();

        // AUTO ALERT LOGIC
        await GenerateAlertsAsync(entity);

        var created = await BaseQuery()
            .FirstOrDefaultAsync(r => r.Rpmmonitoringid == entity.Rpmmonitoringid);

        return RpmmonitoringMapper.ToResponseDto(created!);
    }

    // ---------------- ALERT LOGIC ----------------
    private async Task GenerateAlertsAsync(Rpmmonitoring r)
    {
        var alerts = new List<Patientalert>();

        // BP Alert
        if (r.Systolic >= 160 || r.Diastolic >= 100)
        {
            alerts.Add(CreateAlert(r.Patientid, "Vitals", "High",
                $"High BP detected: {r.Systolic}/{r.Diastolic}"));
        }

        // Low BP
        if (r.Systolic <= 90 || r.Diastolic <= 60)
        {
            alerts.Add(CreateAlert(r.Patientid, "Vitals", "Medium",
                $"Low BP detected: {r.Systolic}/{r.Diastolic}"));
        }

        // SpO2
        if (r.Spo2.HasValue && r.Spo2 < 90)
        {
            alerts.Add(CreateAlert(r.Patientid, "Vitals", "Critical",
                $"Low SpO2: {r.Spo2}%"));
        }

        // Heart Rate
        if (r.Heartrate.HasValue && (r.Heartrate < 50 || r.Heartrate > 120))
        {
            alerts.Add(CreateAlert(r.Patientid, "Vitals", "High",
                $"Abnormal Heart Rate: {r.Heartrate}"));
        }

        // Glucose
        if (r.Glucose.HasValue && (r.Glucose < 70 || r.Glucose > 180))
        {
            alerts.Add(CreateAlert(r.Patientid, "Vitals", "High",
                $"Abnormal Glucose: {r.Glucose} {r.Glucoseunit}"));
        }

        // Temperature
        if (r.Temperature.HasValue && (r.Temperature < 95 || r.Temperature > 100.4m))
        {
            alerts.Add(CreateAlert(r.Patientid, "Vitals", "High",
                $"Abnormal Temperature: {r.Temperature} {r.Temperatureunit}"));
        }

        // Save alerts
        if (alerts.Any())
        {
            _context.Patientalerts.AddRange(alerts);
            await _context.SaveChangesAsync();
        }
    }

    private Patientalert CreateAlert(long patientId, string type, string severity, string message)
    {
        return new Patientalert
        {
            Patientid = patientId,
            Alerttype = type,
            Alertmessage = message,
            Severity = severity,
            Isread = false,
            Isactive = true,
            Isacknowledged = false,
            Createddate = DateTime.UtcNow
        };
    }

    // ---------------- OTHER METHODS ----------------

    public async Task<IEnumerable<RpmmonitoringResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery().OrderByDescending(r => r.Readingdate).ToListAsync();
        return list.Select(RpmmonitoringMapper.ToResponseDto);
    }

    public async Task<RpmmonitoringResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery().FirstOrDefaultAsync(r => r.Rpmmonitoringid == id);
        return entity == null ? null : RpmmonitoringMapper.ToResponseDto(entity);
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
            .ToListAsync();

        return list.Select(RpmmonitoringMapper.ToResponseDto);
    }

    public async Task<IEnumerable<RpmmonitoringResponseDto>> GetUnreviewedByPatientAsync(long patientId)
    {
        var list = await BaseQuery()
            .Where(r => r.Patientid == patientId && r.Isreviewed == false)
            .ToListAsync();

        return list.Select(RpmmonitoringMapper.ToResponseDto);
    }

    public async Task<RpmmonitoringResponseDto?> GetLatestByPatientAsync(long patientId)
    {
        var entity = await BaseQuery()
            .Where(r => r.Patientid == patientId)
            .OrderByDescending(r => r.Readingdate)
            .FirstOrDefaultAsync();

        return entity == null ? null : RpmmonitoringMapper.ToResponseDto(entity);
    }

    public async Task<RpmmonitoringResponseDto?> UpdateAsync(long id, UpdateRpmmonitoringDto dto)
    {
        var entity = await BaseQuery().FirstOrDefaultAsync(r => r.Rpmmonitoringid == id);
        if (entity == null) return null;

        if (entity.Isreviewed == true)
            throw new ArgumentException("Cannot update reviewed record.");

        RpmmonitoringMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();

        return RpmmonitoringMapper.ToResponseDto(entity);
    }

    public async Task<RpmmonitoringResponseDto?> MarkReviewedAsync(long id, RpmReviewDto dto)
    {
        var entity = await BaseQuery().FirstOrDefaultAsync(r => r.Rpmmonitoringid == id);
        if (entity == null) return null;

        entity.Isreviewed = true;
        entity.Reviewedby = dto.Reviewedby;
        entity.Reviewedat = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return RpmmonitoringMapper.ToResponseDto(entity);
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