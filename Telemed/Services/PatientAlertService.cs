// Services/PatientAlertService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PatientAlertService : IPatientAlertService
{
    private readonly TelemedDbContext _context;

    public PatientAlertService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<PatientAlertResponseDto> CreateAsync(
        CreatePatientAlertDto dto)
    {
        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate AlertType
        var validAlertTypes = new[]
        {
            "Critical", "Warning", "Info",
            "Reminder", "Medication", "Appointment",
            "Lab", "Vitals"
        };
        if (!validAlertTypes.Contains(dto.Alerttype,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                "Invalid AlertType. Allowed: Critical, Warning, Info, " +
                "Reminder, Medication, Appointment, Lab, Vitals.");

        // Validate Severity
        if (!string.IsNullOrEmpty(dto.Severity))
        {
            var validSeverities = new[]
            {
                "Low", "Medium", "High", "Critical"
            };
            if (!validSeverities.Contains(dto.Severity,
                StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(
                    "Invalid Severity. Allowed: Low, Medium, High, Critical.");
        }

        var entity = PatientAlertMapper.ToEntity(dto);
        _context.Patientalerts.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.Patientalerts
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Alertid == entity.Alertid);

        return PatientAlertMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<PatientAlertResponseDto>> GetAllAsync()
    {
        var list = await _context.Patientalerts
            .Include(a => a.Patient)
            .Where(a => a.Isactive == true)
            .OrderByDescending(a => a.Createddate)
            .ToListAsync();

        return list.Select(PatientAlertMapper.ToResponseDto);
    }

    public async Task<PatientAlertResponseDto?> GetByIdAsync(long id)
    {
        var alert = await _context.Patientalerts
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Alertid == id);

        if (alert == null) return null;
        return PatientAlertMapper.ToResponseDto(alert);
    }

    public async Task<IEnumerable<PatientAlertResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Patientalerts
            .Include(a => a.Patient)
            .Where(a => a.Patientid == patientId
                     && a.Isactive == true)
            .OrderByDescending(a => a.Createddate)
            .ToListAsync();

        return list.Select(PatientAlertMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAlertResponseDto>> GetUnreadByPatientAsync(
        long patientId)
    {
        var list = await _context.Patientalerts
            .Include(a => a.Patient)
            .Where(a => a.Patientid == patientId
                     && a.Isread == false
                     && a.Isactive == true)
            .OrderByDescending(a => a.Createddate)
            .ToListAsync();

        return list.Select(PatientAlertMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAlertResponseDto>> GetBySeverityAsync(
        string severity)
    {
        var list = await _context.Patientalerts
            .Include(a => a.Patient)
            .Where(a => a.Severity != null &&
                        a.Severity.ToLower() == severity.ToLower()
                     && a.Isactive == true)
            .OrderByDescending(a => a.Createddate)
            .ToListAsync();

        return list.Select(PatientAlertMapper.ToResponseDto);
    }

    public async Task<PatientAlertResponseDto?> UpdateAsync(
        long id, UpdatePatientAlertDto dto)
    {
        var entity = await _context.Patientalerts
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Alertid == id);

        if (entity == null) return null;

        PatientAlertMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return PatientAlertMapper.ToResponseDto(entity);
    }

    public async Task<PatientAlertResponseDto?> MarkAsReadAsync(long id)
    {
        var entity = await _context.Patientalerts
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Alertid == id);

        if (entity == null) return null;

        entity.Isread = true;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return PatientAlertMapper.ToResponseDto(entity);
    }

    public async Task<PatientAlertResponseDto?> AcknowledgeAsync(long id)
    {
        var entity = await _context.Patientalerts
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Alertid == id);

        if (entity == null) return null;

        entity.Isacknowledged = true;
        entity.Isread = true;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return PatientAlertMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Patientalerts.FindAsync(id);
        if (entity == null) return false;

        // Soft delete
        entity.Isactive = false;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return true;
    }
}