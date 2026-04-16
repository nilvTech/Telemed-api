// Services/ClaimService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ClaimService : IClaimService
{
    private readonly TelemedDbContext _context;

    public ClaimService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Claim> BaseQuery()
    {
        return _context.Claims
            .Include(c => c.Patient)
            .Include(c => c.Providerinfo)
            .Include(c => c.Claimdetails);
    }

    private static readonly string[] ValidStatuses = new[]
    {
        "Pending", "Submitted", "InReview",
        "Approved", "Paid", "Denied", "Appealed", "Cancelled"
    };

    public async Task<ClaimResponseDto> CreateAsync(CreateClaimDto dto)
    {
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

        // Validate claim number unique if provided
        if (!string.IsNullOrEmpty(dto.Claimnumber))
        {
            var claimExists = await _context.Claims
                .AnyAsync(c => c.Claimnumber == dto.Claimnumber);
            if (claimExists)
                throw new ArgumentException(
                    $"Claim number '{dto.Claimnumber}' already exists.");
        }

        // Validate at least one detail
        if (!dto.Claimdetails.Any())
            throw new ArgumentException(
                "At least one claim detail (CPT code) is required.");

        // Validate amounts
        foreach (var detail in dto.Claimdetails)
        {
            if (detail.Amount <= 0)
                throw new ArgumentException(
                    $"Amount for CPT code '{detail.Cptcode}' must be greater than 0.");
        }

        var entity = ClaimMapper.ToEntity(dto);
        _context.Claims.Add(entity);
        await _context.SaveChangesAsync();

        // Add claim details
        foreach (var detail in dto.Claimdetails)
        {
            var detailEntity = ClaimMapper.ToDetailEntity(detail, entity.Claimid);
            _context.Claimdetails.Add(detailEntity);
        }
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Claimid == entity.Claimid);

        return ClaimMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<ClaimResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimMapper.ToResponseDto);
    }

    public async Task<ClaimResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Claimid == id);

        if (entity == null) return null;
        return ClaimMapper.ToResponseDto(entity);
    }

    public async Task<ClaimResponseDto?> GetByClaimnumberAsync(string claimnumber)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Claimnumber == claimnumber);

        if (entity == null) return null;
        return ClaimMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<ClaimResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(c => c.Patientid == patientId)
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ClaimResponseDto>> GetByProviderIdAsync(
        long providerInfoId)
    {
        var list = await BaseQuery()
            .Where(c => c.Providerinfoid == providerInfoId)
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ClaimResponseDto>> GetByStatusAsync(string status)
    {
        var list = await BaseQuery()
            .Where(c => c.Status != null &&
                        c.Status.ToLower() == status.ToLower())
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ClaimResponseDto>> GetByPayerAsync(string payer)
    {
        var list = await BaseQuery()
            .Where(c => c.Payer != null &&
                        c.Payer.ToLower().Contains(payer.ToLower()))
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimMapper.ToResponseDto);
    }

    public async Task<ClaimResponseDto?> UpdateAsync(long id, UpdateClaimDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Claimid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        ClaimMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ClaimMapper.ToResponseDto(entity);
    }

    public async Task<ClaimResponseDto?> UpdateStatusAsync(
        long id, ClaimStatusUpdateDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Claimid == id);

        if (entity == null) return null;

        if (!ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        entity.Status = dto.Status;
        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        if (dto.Paidamount.HasValue)
            entity.Paidamount = dto.Paidamount;

        if (!string.IsNullOrEmpty(dto.Deniedreason))
            entity.Deniedreason = dto.Deniedreason;

        await _context.SaveChangesAsync();
        return ClaimMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Claims.FindAsync(id);
        if (entity == null) return false;

        _context.Claims.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // ========== Claim Detail Methods ==========

    public async Task<ClaimDetailResponseDto> AddDetailAsync(
        long claimId, CreateClaimDetailDto dto)
    {
        var claimExists = await _context.Claims
            .AnyAsync(c => c.Claimid == claimId);
        if (!claimExists)
            throw new ArgumentException(
                $"Claim with ID {claimId} does not exist.");

        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0.");

        var entity = ClaimMapper.ToDetailEntity(dto, claimId);
        _context.Claimdetails.Add(entity);
        await _context.SaveChangesAsync();

        // Recalculate total amount
        await RecalculateTotalAsync(claimId);

        return ClaimMapper.ToDetailResponseDto(entity);
    }

    public async Task<ClaimDetailResponseDto?> UpdateDetailAsync(
        long detailId, UpdateClaimDetailDto dto)
    {
        var entity = await _context.Claimdetails
            .FirstOrDefaultAsync(d => d.Claimdetailid == detailId);

        if (entity == null) return null;

        ClaimMapper.UpdateDetailEntity(entity, dto);
        await _context.SaveChangesAsync();

        // Recalculate total amount
        await RecalculateTotalAsync(entity.Claimid);

        return ClaimMapper.ToDetailResponseDto(entity);
    }

    public async Task<bool> DeleteDetailAsync(long detailId)
    {
        var entity = await _context.Claimdetails.FindAsync(detailId);
        if (entity == null) return false;

        var claimId = entity.Claimid;
        _context.Claimdetails.Remove(entity);
        await _context.SaveChangesAsync();

        // Recalculate total amount after delete
        await RecalculateTotalAsync(claimId);

        return true;
    }

    // Helper — recalculate total when details change
    private async Task RecalculateTotalAsync(long claimId)
    {
        var claim = await _context.Claims
            .Include(c => c.Claimdetails)
            .FirstOrDefaultAsync(c => c.Claimid == claimId);

        if (claim == null) return;

        claim.Totalamount = claim.Claimdetails
            .Sum(d => d.Amount * (d.Units ?? 1));

        claim.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
    }
}