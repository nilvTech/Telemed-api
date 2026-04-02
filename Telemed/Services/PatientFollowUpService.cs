// Services/PatientFollowUpService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PatientFollowUpService : IPatientFollowUpService
{
    private readonly TelemedDbContext _context;

    public PatientFollowUpService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<PatientFollowUpResponseDto> CreateAsync(
        CreatePatientFollowUpDto dto)
    {
        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate date not in past
        if (dto.Followupdate.Date < DateTime.Today)
            throw new ArgumentException(
                "Follow-up date cannot be in the past.");

        var entity = PatientFollowUpMapper.ToEntity(dto);
        _context.Patientfollowups.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.Patientfollowups
            .Include(f => f.Patient)
            .FirstOrDefaultAsync(f => f.Id == entity.Id);

        return PatientFollowUpMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<PatientFollowUpResponseDto>> GetAllAsync()
    {
        var list = await _context.Patientfollowups
            .Include(f => f.Patient)
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(PatientFollowUpMapper.ToResponseDto);
    }

    public async Task<PatientFollowUpResponseDto?> GetByIdAsync(long id)
    {
        var followup = await _context.Patientfollowups
            .Include(f => f.Patient)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (followup == null) return null;
        return PatientFollowUpMapper.ToResponseDto(followup);
    }

    public async Task<IEnumerable<PatientFollowUpResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Patientfollowups
            .Include(f => f.Patient)
            .Where(f => f.Patientid == patientId)
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(PatientFollowUpMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientFollowUpResponseDto>> GetByStatusAsync(
        string status)
    {
        var list = await _context.Patientfollowups
            .Include(f => f.Patient)
            .Where(f => f.Status != null &&
                        f.Status.ToLower() == status.ToLower())
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(PatientFollowUpMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientFollowUpResponseDto>> GetUpcomingAsync()
    {
        var now = DateTime.Today;

        var list = await _context.Patientfollowups
            .Include(f => f.Patient)
            .Where(f => f.Followupdate >= now
                     && f.Status == "Pending")
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(PatientFollowUpMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientFollowUpResponseDto>> GetOverdueAsync()
    {
        var now = DateTime.Today;

        var list = await _context.Patientfollowups
            .Include(f => f.Patient)
            .Where(f => f.Followupdate < now
                     && f.Status == "Pending")
            .OrderBy(f => f.Followupdate)
            .ToListAsync();

        return list.Select(PatientFollowUpMapper.ToResponseDto);
    }

    public async Task<PatientFollowUpResponseDto?> UpdateAsync(
        long id, UpdatePatientFollowUpDto dto)
    {
        var entity = await _context.Patientfollowups
            .Include(f => f.Patient)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status))
        {
            var validStatuses = new[]
            {
                "Pending", "Completed", "Cancelled", "Missed"
            };
            if (!validStatuses.Contains(dto.Status,
                StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(
                    "Invalid Status. Allowed: Pending, Completed, Cancelled, Missed.");
        }

        PatientFollowUpMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return PatientFollowUpMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Patientfollowups.FindAsync(id);
        if (entity == null) return false;

        _context.Patientfollowups.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}