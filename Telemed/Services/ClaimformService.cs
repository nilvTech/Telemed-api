// Services/ClaimformService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ClaimformService : IClaimformService
{
    private readonly TelemedDbContext _context;

    public ClaimformService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<ClaimformResponseDto> CreateAsync(CreateClaimformDto dto)
    {
        // Validate patient name required
        if (string.IsNullOrWhiteSpace(dto.Patientname))
            throw new ArgumentException("Patient name is required.");

        // Validate service amount
        if (dto.Serviceamount.HasValue && dto.Serviceamount < 0)
            throw new ArgumentException(
                "Service amount cannot be negative.");

        // Validate service date not in future
        if (dto.Servicedate.HasValue &&
            dto.Servicedate > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException(
                "Service date cannot be in the future.");

        // Validate date of birth not in future
        if (dto.Dateofbirth.HasValue &&
            dto.Dateofbirth > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException(
                "Date of birth cannot be in the future.");

        // Validate date of illness not in future
        if (dto.Dateofillness.HasValue &&
            dto.Dateofillness > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException(
                "Date of illness cannot be in the future.");

        // Validate gender
        if (!string.IsNullOrEmpty(dto.Gender))
        {
            var validGenders = new[] { "Male", "Female", "Other" };
            if (!validGenders.Contains(dto.Gender,
                StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(
                    "Invalid Gender. Allowed: Male, Female, Other.");
        }

        var entity = ClaimformMapper.ToEntity(dto);
        _context.Claimforms.Add(entity);
        await _context.SaveChangesAsync();

        return ClaimformMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<ClaimformResponseDto>> GetAllAsync()
    {
        var list = await _context.Claimforms
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimformMapper.ToResponseDto);
    }

    public async Task<ClaimformResponseDto?> GetByIdAsync(long id)
    {
        var entity = await _context.Claimforms
            .FirstOrDefaultAsync(c => c.Claimformid == id);

        if (entity == null) return null;
        return ClaimformMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<ClaimformResponseDto>> GetByPatientnameAsync(
        string patientname)
    {
        var list = await _context.Claimforms
            .Where(c => c.Patientname != null &&
                        c.Patientname.ToLower()
                         .Contains(patientname.ToLower()))
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimformMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ClaimformResponseDto>> GetByPolicynumberAsync(
        string policynumber)
    {
        var list = await _context.Claimforms
            .Where(c => c.Policynumber != null &&
                        c.Policynumber.ToLower() ==
                        policynumber.ToLower())
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimformMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ClaimformResponseDto>> GetByDiagnosiscodeAsync(
        string diagnosiscode)
    {
        var list = await _context.Claimforms
            .Where(c => c.Diagnosiscode != null &&
                        c.Diagnosiscode.ToLower() ==
                        diagnosiscode.ToLower())
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimformMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ClaimformResponseDto>> GetByServicedateAsync(
        DateOnly servicedate)
    {
        var list = await _context.Claimforms
            .Where(c => c.Servicedate == servicedate)
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimformMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ClaimformResponseDto>> GetByDateRangeAsync(
        DateOnly from, DateOnly to)
    {
        var list = await _context.Claimforms
            .Where(c => c.Servicedate.HasValue &&
                        c.Servicedate >= from &&
                        c.Servicedate <= to)
            .OrderByDescending(c => c.Servicedate)
            .ToListAsync();

        return list.Select(ClaimformMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ClaimformResponseDto>> SearchAsync(
        string keyword)
    {
        var list = await _context.Claimforms
            .Where(c =>
                (c.Patientname != null &&
                 c.Patientname.ToLower().Contains(keyword.ToLower())) ||
                (c.Policynumber != null &&
                 c.Policynumber.ToLower().Contains(keyword.ToLower())) ||
                (c.Diagnosiscode != null &&
                 c.Diagnosiscode.ToLower().Contains(keyword.ToLower())) ||
                (c.Servicecptcode != null &&
                 c.Servicecptcode.ToLower().Contains(keyword.ToLower())) ||
                (c.Referringprovider != null &&
                 c.Referringprovider.ToLower().Contains(keyword.ToLower())))
            .OrderByDescending(c => c.Createdat)
            .ToListAsync();

        return list.Select(ClaimformMapper.ToResponseDto);
    }

    public async Task<ClaimformResponseDto?> UpdateAsync(
        long id, UpdateClaimformDto dto)
    {
        var entity = await _context.Claimforms
            .FirstOrDefaultAsync(c => c.Claimformid == id);

        if (entity == null) return null;

        if (dto.Serviceamount.HasValue && dto.Serviceamount < 0)
            throw new ArgumentException(
                "Service amount cannot be negative.");

        if (dto.Servicedate.HasValue &&
            dto.Servicedate > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException(
                "Service date cannot be in the future.");

        ClaimformMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ClaimformMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Claimforms.FindAsync(id);
        if (entity == null) return false;

        _context.Claimforms.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}