// Services/PatientSummaryService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PatientsSummaryService : IPatientsSummaryService
{
    private readonly TelemedDbContext _context;

    public PatientsSummaryService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientsSummaryResponseDto>> GetAllAsync()
    {
        var list = await _context.Patientssummaries
            .OrderBy(p => p.Lastname)
            .ThenBy(p => p.Firstname)
            .ToListAsync();

        return list.Select(PatientsSummaryMapper.ToResponseDto);
    }

    public async Task<PatientsSummaryResponseDto?> GetByPatientIdAsync(
        long patientId)
    {
        var entity = await _context.Patientssummaries
            .FirstOrDefaultAsync(p => p.Patientid == patientId);

        if (entity == null) return null;
        return PatientsSummaryMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<PatientsSummaryResponseDto>> GetByGenderAsync(
        string gender)
    {
        var list = await _context.Patientssummaries
            .Where(p => p.Gender != null &&
                        p.Gender.ToLower() == gender.ToLower())
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(PatientsSummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientsSummaryResponseDto>> GetByConditionAsync(
        string condition)
    {
        var list = await _context.Patientssummaries
            .Where(p => p.Conditions != null &&
                        p.Conditions.ToLower()
                         .Contains(condition.ToLower()))
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(PatientsSummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientsSummaryResponseDto>> SearchAsync(
        string keyword)
    {
        var list = await _context.Patientssummaries
            .Where(p =>
                (p.Firstname != null &&
                 p.Firstname.ToLower().Contains(keyword.ToLower())) ||
                (p.Lastname != null &&
                 p.Lastname.ToLower().Contains(keyword.ToLower())) ||
                (p.Conditions != null &&
                 p.Conditions.ToLower().Contains(keyword.ToLower())))
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(PatientsSummaryMapper.ToResponseDto);
    }
}