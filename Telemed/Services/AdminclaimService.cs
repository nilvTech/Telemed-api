// Services/AdminclaimService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class AdminclaimService : IAdminclaimService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidStatuses = new[]
    {
        "Submitted", "UnderReview", "Approved",
        "Rejected", "Appealed", "Paid", "Closed"
    };

    private static readonly string[] ValidLastActions = new[]
    {
        "Submitted", "Reviewed", "Approved",
        "Rejected", "Appealed", "PaymentProcessed", "Closed"
    };

    public AdminclaimService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Adminclaim> BaseQuery()
    {
        return _context.Adminclaims
            .Include(a => a.Claim)
            .Include(a => a.Patient)
            .Include(a => a.Providerinfo)
                .ThenInclude(p => p!.Providerprofile)
            .Include(a => a.Appointment)
            .Include(a => a.Encounter);
    }

    public async Task<AdminclaimResponseDto> CreateAsync(
        CreateAdminclaimDto dto)
    {
        // Validate Claim exists
        var claim = await _context.Claims
            .FirstOrDefaultAsync(c => c.Claimid == dto.Claimid);
        if (claim == null)
            throw new ArgumentException(
                $"Claim with ID {dto.Claimid} does not exist.");

        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate Provider exists
        var providerExists = await _context.Providerinfos
            .AnyAsync(p => p.Providerinfoid == dto.Providerinfoid);
        if (!providerExists)
            throw new ArgumentException(
                $"Provider with ID {dto.Providerinfoid} does not exist.");

        // Validate Appointment if provided
        if (dto.Appointmentid.HasValue)
        {
            var appointmentExists = await _context.Appointments
                .AnyAsync(a => a.Appointmentid == dto.Appointmentid);
            if (!appointmentExists)
                throw new ArgumentException(
                    $"Appointment with ID {dto.Appointmentid} does not exist.");
        }

        // Validate Encounter if provided
        if (dto.Encounterid.HasValue)
        {
            var encounterExists = await _context.Encounters
                .AnyAsync(e => e.Encounterid == dto.Encounterid);
            if (!encounterExists)
                throw new ArgumentException(
                    $"Encounter with ID {dto.Encounterid} does not exist.");
        }

        // Validate Status
        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        // Validate no duplicate admin claim for same claim
        var duplicateExists = await _context.Adminclaims
            .AnyAsync(a => a.Claimid == dto.Claimid);
        if (duplicateExists)
            throw new ArgumentException(
                $"An admin claim already exists for claim ID {dto.Claimid}.");

        var entity = AdminclaimMapper.ToEntity(dto);
        _context.Adminclaims.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(a =>
                a.Adminclaimid == entity.Adminclaimid);

        return AdminclaimMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<AdminclaimResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderByDescending(a => a.Createdat)
            .ToListAsync();

        return list.Select(AdminclaimMapper.ToResponseDto);
    }

    public async Task<AdminclaimResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(a => a.Adminclaimid == id);

        if (entity == null) return null;
        return AdminclaimMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<AdminclaimResponseDto>> GetByClaimIdAsync(
        long claimId)
    {
        var list = await BaseQuery()
            .Where(a => a.Claimid == claimId)
            .OrderByDescending(a => a.Createdat)
            .ToListAsync();

        return list.Select(AdminclaimMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AdminclaimResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(a => a.Patientid == patientId)
            .OrderByDescending(a => a.Createdat)
            .ToListAsync();

        return list.Select(AdminclaimMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AdminclaimResponseDto>> GetByProviderIdAsync(
        long providerInfoId)
    {
        var list = await BaseQuery()
            .Where(a => a.Providerinfoid == providerInfoId)
            .OrderByDescending(a => a.Createdat)
            .ToListAsync();

        return list.Select(AdminclaimMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AdminclaimResponseDto>> GetByAppointmentIdAsync(
        long appointmentId)
    {
        var list = await BaseQuery()
            .Where(a => a.Appointmentid == appointmentId)
            .OrderByDescending(a => a.Createdat)
            .ToListAsync();

        return list.Select(AdminclaimMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AdminclaimResponseDto>> GetByEncounterIdAsync(
        int encounterId)
    {
        var list = await BaseQuery()
            .Where(a => a.Encounterid == encounterId)
            .OrderByDescending(a => a.Createdat)
            .ToListAsync();

        return list.Select(AdminclaimMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AdminclaimResponseDto>> GetByStatusAsync(
        string status)
    {
        var list = await BaseQuery()
            .Where(a => a.Status != null &&
                        a.Status.ToLower() == status.ToLower())
            .OrderByDescending(a => a.Createdat)
            .ToListAsync();

        return list.Select(AdminclaimMapper.ToResponseDto);
    }

    public async Task<AdminclaimResponseDto?> UpdateAsync(
        long id, UpdateAdminclaimDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(a => a.Adminclaimid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        if (!string.IsNullOrEmpty(dto.Lastaction) &&
            !ValidLastActions.Contains(dto.Lastaction,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Last Action. Allowed: " +
                $"{string.Join(", ", ValidLastActions)}.");

        AdminclaimMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return AdminclaimMapper.ToResponseDto(entity);
    }

    public async Task<AdminclaimResponseDto?> UpdateStatusAsync(
        long id, AdminclaimStatusUpdateDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(a => a.Adminclaimid == id);

        if (entity == null) return null;

        if (!ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        entity.Status = dto.Status;
        entity.Lastaction = dto.Lastaction ?? dto.Status;
        entity.Lastactiondate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return AdminclaimMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Adminclaims.FindAsync(id);
        if (entity == null) return false;

        if (entity.Status == "Paid" || entity.Status == "Closed")
            throw new ArgumentException(
                "Cannot delete a Paid or Closed admin claim.");

        _context.Adminclaims.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}