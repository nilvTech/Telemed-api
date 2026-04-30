// Services/BillingclaimsummaryService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class BillingclaimsummaryService : IBillingclaimsummaryService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidStatuses = new[]
    {
        "Pending", "Submitted", "InReview",
        "Approved", "Paid", "Denied",
        "Appealed", "Cancelled"
    };

    public BillingclaimsummaryService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Billingclaimsummary> BaseQuery()
    {
        return _context.Billingclaimsummaries
            .Include(b => b.Claim)
            .Include(b => b.Patient)
            .Include(b => b.Providerinfo)
                .ThenInclude(p => p!.Providerprofile);
    }

    public async Task<BillingclaimsummaryResponseDto> CreateAsync(
        CreateBillingclaimsummaryDto dto)
    {
        // Validate Claim exists
        var claimExists = await _context.Claims
            .AnyAsync(c => c.Claimid == dto.Claimid);
        if (!claimExists)
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

        // Validate Status
        if (!ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        // Validate Amount
        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0.");

        var entity = BillingclaimsummaryMapper.ToEntity(dto);
        _context.Billingclaimsummaries.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(b => b.Summaryid == entity.Summaryid);

        return BillingclaimsummaryMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<BillingclaimsummaryResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderByDescending(b => b.Claimdate)
            .ToListAsync();

        return list.Select(BillingclaimsummaryMapper.ToResponseDto);
    }

    public async Task<BillingclaimsummaryResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(b => b.Summaryid == id);

        if (entity == null) return null;
        return BillingclaimsummaryMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByClaimIdAsync(
        long claimId)
    {
        var list = await BaseQuery()
            .Where(b => b.Claimid == claimId)
            .OrderByDescending(b => b.Claimdate)
            .ToListAsync();

        return list.Select(BillingclaimsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(b => b.Patientid == patientId)
            .OrderByDescending(b => b.Claimdate)
            .ToListAsync();

        return list.Select(BillingclaimsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByProviderIdAsync(
        long providerInfoId)
    {
        var list = await BaseQuery()
            .Where(b => b.Providerinfoid == providerInfoId)
            .OrderByDescending(b => b.Claimdate)
            .ToListAsync();

        return list.Select(BillingclaimsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByStatusAsync(
        string status)
    {
        var list = await BaseQuery()
            .Where(b => b.Status.ToLower() == status.ToLower())
            .OrderByDescending(b => b.Claimdate)
            .ToListAsync();

        return list.Select(BillingclaimsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByCptcodeAsync(
        string cptcode)
    {
        var list = await BaseQuery()
            .Where(b => b.Cptcode != null &&
                        b.Cptcode.ToLower() == cptcode.ToLower())
            .OrderByDescending(b => b.Claimdate)
            .ToListAsync();

        return list.Select(BillingclaimsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<BillingclaimsummaryResponseDto>> GetByDateRangeAsync(
        DateTime from, DateTime to)
    {
        var list = await BaseQuery()
            .Where(b => b.Claimdate >= from &&
                        b.Claimdate <= to)
            .OrderByDescending(b => b.Claimdate)
            .ToListAsync();

        return list.Select(BillingclaimsummaryMapper.ToResponseDto);
    }

    public async Task<BillingSummaryStatsDto> GetStatsAsync()
    {
        var all = await _context.Billingclaimsummaries.ToListAsync();
        return BuildStats(all);
    }

    public async Task<BillingSummaryStatsDto> GetStatsByProviderAsync(
        long providerInfoId)
    {
        var list = await _context.Billingclaimsummaries
            .Where(b => b.Providerinfoid == providerInfoId)
            .ToListAsync();

        return BuildStats(list);
    }

    public async Task<BillingSummaryStatsDto> GetStatsByPatientAsync(
        long patientId)
    {
        var list = await _context.Billingclaimsummaries
            .Where(b => b.Patientid == patientId)
            .ToListAsync();

        return BuildStats(list);
    }

    private static BillingSummaryStatsDto BuildStats(
        List<Billingclaimsummary> list)
    {
        var claimsByStatus = list
            .GroupBy(b => b.Status)
            .ToDictionary(g => g.Key, g => g.Count());

        var amountByStatus = list
            .GroupBy(b => b.Status)
            .ToDictionary(g => g.Key, g => g.Sum(b => b.Amount));

        return new BillingSummaryStatsDto
        {
            TotalClaims = list.Count,
            TotalAmount = list.Sum(b => b.Amount),
            PaidAmount = list.Where(b => b.Status == "Paid")
                                 .Sum(b => b.Amount),
            PendingAmount = list.Where(b => b.Status == "Pending" ||
                                             b.Status == "Submitted" ||
                                             b.Status == "InReview")
                                 .Sum(b => b.Amount),
            DeniedAmount = list.Where(b => b.Status == "Denied")
                                 .Sum(b => b.Amount),
            ClaimsByStatus = claimsByStatus,
            AmountByStatus = amountByStatus
        };
    }

    public async Task<BillingclaimsummaryResponseDto?> UpdateAsync(
        long id, UpdateBillingclaimsummaryDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(b => b.Summaryid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        if (dto.Amount.HasValue && dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0.");

        BillingclaimsummaryMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return BillingclaimsummaryMapper.ToResponseDto(entity);
    }

    public async Task<BillingclaimsummaryResponseDto?> UpdateStatusAsync(
        long id, BillingStatusUpdateDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(b => b.Summaryid == id);

        if (entity == null) return null;

        if (!ValidStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        entity.Status = dto.Status;
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return BillingclaimsummaryMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Billingclaimsummaries.FindAsync(id);
        if (entity == null) return false;

        if (entity.Status == "Paid")
            throw new ArgumentException(
                "Cannot delete a Paid billing summary.");

        _context.Billingclaimsummaries.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // Billing Cliam View

   
}