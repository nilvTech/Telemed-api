// Services/ClinicalOrderService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ClinicalOrderService : IClinicalOrderService
{
    private readonly TelemedDbContext _context;

    private static readonly string[] ValidOrderTypes = new[]
    {
        "Lab", "Imaging", "Medication"
    };

    private static readonly string[] ValidPriorities = new[]
    {
        "Routine", "Urgent", "STAT"
    };

    private static readonly string[] ValidStatuses = new[]
    {
        "Pending", "InProgress", "Completed", "Cancelled"
    };

    public ClinicalOrderService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Clinicalorder> BaseOrderQuery()
    {
        return _context.Clinicalorders
            .Include(o => o.Patient)
            .Include(o => o.Providerinfo)
                .ThenInclude(p => p!.Providerprofile)
            .Include(o => o.Clinicalmaster)
            .Include(o => o.Encounter);
         // ✅ ADD THIS ONLY
    }

    // ===================== MASTER =====================

    public async Task<ClinicalMasterResponseDto> CreateMasterAsync(
        CreateClinicalMasterDto dto)
    {
        // Validate order type
        if (!ValidOrderTypes.Contains(dto.Ordertype,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Order Type. Allowed: {string.Join(", ", ValidOrderTypes)}.");

        // Validate order code unique
        var codeExists = await _context.Clinicalmasters
            .AnyAsync(m => m.Ordercode == dto.Ordercode);
        if (codeExists)
            throw new ArgumentException(
                $"Order code '{dto.Ordercode}' already exists.");

        var entity = ClinicalOrderMapper.ToMasterEntity(dto);
        _context.Clinicalmasters.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.Clinicalmasters
            .Include(m => m.Clinicalorders)
            .FirstOrDefaultAsync(m =>
                m.Clinicalmasterid == entity.Clinicalmasterid);

        return ClinicalOrderMapper.ToMasterResponseDto(created!);
    }

    public async Task<IEnumerable<ClinicalMasterResponseDto>> GetAllMastersAsync()
    {
        var list = await _context.Clinicalmasters
            .Include(m => m.Clinicalorders)
            .OrderBy(m => m.Ordertype)
            .ThenBy(m => m.Ordername)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToMasterResponseDto);
    }

    public async Task<ClinicalMasterResponseDto?> GetMasterByIdAsync(long id)
    {
        var entity = await _context.Clinicalmasters
            .Include(m => m.Clinicalorders)
            .FirstOrDefaultAsync(m => m.Clinicalmasterid == id);

        if (entity == null) return null;
        return ClinicalOrderMapper.ToMasterResponseDto(entity);
    }

    public async Task<IEnumerable<ClinicalMasterResponseDto>> GetMastersByTypeAsync(
        string ordertype)
    {
        var list = await _context.Clinicalmasters
            .Include(m => m.Clinicalorders)
            .Where(m => m.Ordertype.ToLower() == ordertype.ToLower())
            .OrderBy(m => m.Ordername)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToMasterResponseDto);
    }

    public async Task<IEnumerable<ClinicalMasterResponseDto>> SearchMastersAsync(
        string keyword)
    {
        var list = await _context.Clinicalmasters
            .Include(m => m.Clinicalorders)
            .Where(m => m.Ordername.ToLower().Contains(keyword.ToLower()) ||
                        m.Ordercode.ToLower().Contains(keyword.ToLower()) ||
                        (m.Description != null &&
                         m.Description.ToLower().Contains(keyword.ToLower())))
            .OrderBy(m => m.Ordername)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToMasterResponseDto);
    }

    public async Task<ClinicalMasterResponseDto?> UpdateMasterAsync(
        long id, UpdateClinicalMasterDto dto)
    {
        var entity = await _context.Clinicalmasters
            .Include(m => m.Clinicalorders)
            .FirstOrDefaultAsync(m => m.Clinicalmasterid == id);

        if (entity == null) return null;

        ClinicalOrderMapper.UpdateMasterEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ClinicalOrderMapper.ToMasterResponseDto(entity);
    }

    public async Task<bool> DeleteMasterAsync(long id)
    {
        var entity = await _context.Clinicalmasters.FindAsync(id);
        if (entity == null) return false;

        // Cannot delete if active orders exist
        var hasOrders = await _context.Clinicalorders
            .AnyAsync(o => o.Clinicalmasterid == id &&
                           o.Status != "Completed" &&
                           o.Status != "Cancelled");
        if (hasOrders)
            throw new ArgumentException(
                "Cannot delete master — it has active orders.");

        _context.Clinicalmasters.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // ===================== ORDER =====================

    public async Task<ClinicalOrderResponseDto> CreateOrderAsync(
        CreateClinicalOrderDto dto)
    {
        // Validate Patient
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate Provider
        var providerExists = await _context.Providerinfos
            .AnyAsync(p => p.Providerinfoid == dto.Providerinfoid);
        if (!providerExists)
            throw new ArgumentException(
                $"Provider with ID {dto.Providerinfoid} does not exist.");

        // Validate Master
        var masterExists = await _context.Clinicalmasters
            .AnyAsync(m => m.Clinicalmasterid == dto.Clinicalmasterid);
        if (!masterExists)
            throw new ArgumentException(
                $"Clinical master with ID {dto.Clinicalmasterid} does not exist.");

        // Validate Encounter if provided
        if (dto.Encounterid.HasValue)
        {
            var encounterExists = await _context.Encounters
                .AnyAsync(e => e.Encounterid == dto.Encounterid);
            if (!encounterExists)
                throw new ArgumentException(
                    $"Encounter with ID {dto.Encounterid} does not exist.");
        }

        // Validate Priority
        if (!string.IsNullOrEmpty(dto.Priority) &&
            !ValidPriorities.Contains(dto.Priority,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Priority. Allowed: {string.Join(", ", ValidPriorities)}.");

        var entity = ClinicalOrderMapper.ToOrderEntity(dto);
        _context.Clinicalorders.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseOrderQuery()
            .FirstOrDefaultAsync(o =>
                o.Clinicalorderid == entity.Clinicalorderid);

        return ClinicalOrderMapper.ToOrderResponseDto(created!);
    }

    public async Task<IEnumerable<ClinicalOrderResponseDto>> GetAllOrdersAsync()
    {
        var list = await BaseOrderQuery()
            .OrderByDescending(o => o.Orderdate)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToOrderResponseDto);
    }

    public async Task<ClinicalOrderResponseDto?> GetOrderByIdAsync(long id)
    {
        var entity = await BaseOrderQuery()
            .FirstOrDefaultAsync(o => o.Clinicalorderid == id);

        if (entity == null) return null;
        return ClinicalOrderMapper.ToOrderResponseDto(entity);
    }

    public async Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByPatientIdAsync(
        long patientId)
    {
        var list = await BaseOrderQuery()
            .Where(o => o.Patientid == patientId)
            .OrderByDescending(o => o.Orderdate)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToOrderResponseDto);
    }

    public async Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByProviderIdAsync(
        long providerInfoId)
    {
        var list = await BaseOrderQuery()
            .Where(o => o.Providerinfoid == providerInfoId)
            .OrderByDescending(o => o.Orderdate)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToOrderResponseDto);
    }

    public async Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByEncounterIdAsync(
        int encounterId)
    {
        var list = await BaseOrderQuery()
            .Where(o => o.Encounterid == encounterId)
            .OrderByDescending(o => o.Orderdate)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToOrderResponseDto);
    }

    public async Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByStatusAsync(
        string status)
    {
        var list = await BaseOrderQuery()
            .Where(o => o.Status != null &&
                        o.Status.ToLower() == status.ToLower())
            .OrderByDescending(o => o.Orderdate)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToOrderResponseDto);
    }

    public async Task<IEnumerable<ClinicalOrderResponseDto>> GetOrdersByTypeAsync(
        string ordertype)
    {
        var list = await BaseOrderQuery()
            .Where(o => o.Clinicalmaster != null &&
                        o.Clinicalmaster.Ordertype.ToLower() ==
                        ordertype.ToLower())
            .OrderByDescending(o => o.Orderdate)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToOrderResponseDto);
    }

    public async Task<IEnumerable<ClinicalOrderResponseDto>> GetUrgentOrdersAsync()
    {
        var list = await BaseOrderQuery()
            .Where(o => (o.Priority == "STAT" || o.Priority == "Urgent") &&
                        o.Status == "Pending")
            .OrderByDescending(o => o.Orderdate)
            .ToListAsync();

        return list.Select(ClinicalOrderMapper.ToOrderResponseDto);
    }

    public async Task<ClinicalOrderResponseDto?> UpdateOrderAsync(
        long id, UpdateClinicalOrderDto dto)
    {
        var entity = await BaseOrderQuery()
            .FirstOrDefaultAsync(o => o.Clinicalorderid == id);

        if (entity == null) return null;

        if (entity.Status == "Completed")
            throw new ArgumentException(
                "Cannot update a completed order.");

        if (entity.Status == "Cancelled")
            throw new ArgumentException(
                "Cannot update a cancelled order.");

        if (!string.IsNullOrEmpty(dto.Priority) &&
            !ValidPriorities.Contains(dto.Priority,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Priority. Allowed: {string.Join(", ", ValidPriorities)}.");

        if (!string.IsNullOrEmpty(dto.Status) &&
            !ValidStatuses.Contains(dto.Status,
                StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        ClinicalOrderMapper.UpdateOrderEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ClinicalOrderMapper.ToOrderResponseDto(entity);
    }

    public async Task<ClinicalOrderResponseDto?> UpdateOrderStatusAsync(
        long id, ClinicalOrderStatusUpdateDto dto)
    {
        var entity = await BaseOrderQuery()
            .FirstOrDefaultAsync(o => o.Clinicalorderid == id);

        if (entity == null) return null;

        if (!ValidStatuses.Contains(dto.Status,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                $"Invalid Status. Allowed: {string.Join(", ", ValidStatuses)}.");

        entity.Status = dto.Status;
        entity.Updatedat = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        if (dto.Status.ToLower() == "completed")
            entity.Completeddate = dto.Completeddate.HasValue
                ? DateTime.SpecifyKind(
                    dto.Completeddate.Value,
                    DateTimeKind.Unspecified)
                : DateTime.SpecifyKind(
                    DateTime.UtcNow,
                    DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return ClinicalOrderMapper.ToOrderResponseDto(entity);
    }

    public async Task<bool> DeleteOrderAsync(long id)
    {
        var entity = await _context.Clinicalorders.FindAsync(id);
        if (entity == null) return false;

        if (entity.Status == "Completed")
            throw new ArgumentException(
                "Cannot delete a completed order.");

        _context.Clinicalorders.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // FILE MASTER
    public async Task<Filemaster> UploadOrderFileAsync(long orderId, IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var entity = new Filemaster
        {
            Filename = file.FileName,
            Filetype = $"ORDER-{orderId}",   // Linking without DB column
            Totalsize = ms.Length,
            Uploadedchunks = 1,
            Totalchunks = 1,
            Iscompleted = true,
            Createddate = DateTime.UtcNow,
            Pdfcontent = new List<byte[]> { ms.ToArray() }
        };

        _context.Filemasters.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
}