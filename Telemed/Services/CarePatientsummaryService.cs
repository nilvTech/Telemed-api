// Services/CarePatientsummaryService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class CarePatientsummaryService : ICarePatientsummaryService
{
    private readonly TelemedDbContext _context;

    public CarePatientsummaryService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> GetAllAsync()
    {
        var list = await _context.Carepatientsummaries
            .OrderBy(p => p.Lastname)
            .ThenBy(p => p.Firstname)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }

    public async Task<CarePatientsummaryResponseDto?> GetByPatientIdAsync(
        long patientId)
    {
        var entity = await _context.Carepatientsummaries
            .FirstOrDefaultAsync(p => p.Patientid == patientId);

        if (entity == null) return null;
        return CarePatientsummaryMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> GetByGenderAsync(
        string gender)
    {
        var list = await _context.Carepatientsummaries
            .Where(p => p.Gender != null &&
                        p.Gender.ToLower() == gender.ToLower())
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> GetByConditionAsync(
        string condition)
    {
        var list = await _context.Carepatientsummaries
            .Where(p => p.Conditions != null &&
                        p.Conditions.ToLower()
                         .Contains(condition.ToLower()))
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> GetWithAlertsAsync()
    {
        var list = await _context.Carepatientsummaries
            .Where(p => p.Alerttype != null)
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> GetWithActiveCareplanAsync()
    {
        var list = await _context.Carepatientsummaries
            .Where(p => p.CareplanStatus == "Active")
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> GetWithOverdueTasksAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.Carepatientsummaries
            .Where(p => p.Duedate.HasValue &&
                        p.Duedate < today &&
                        p.TaskStatus != "Completed" &&
                        p.TaskStatus != "Cancelled")
            .OrderBy(p => p.Duedate)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> GetWithOverdueFollowupsAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.Carepatientsummaries
            .Where(p => p.Followupdate.HasValue &&
                        p.Followupdate < today &&
                        p.FollowupStatus == "Scheduled")
            .OrderBy(p => p.Followupdate)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> GetByCareplanStatusAsync(
        string status)
    {
        var list = await _context.Carepatientsummaries
            .Where(p => p.CareplanStatus != null &&
                        p.CareplanStatus.ToLower() == status.ToLower())
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CarePatientsummaryResponseDto>> SearchAsync(
        string keyword)
    {
        var list = await _context.Carepatientsummaries
            .Where(p =>
                (p.Firstname != null &&
                 p.Firstname.ToLower().Contains(keyword.ToLower())) ||
                (p.Lastname != null &&
                 p.Lastname.ToLower().Contains(keyword.ToLower())) ||
                (p.Conditions != null &&
                 p.Conditions.ToLower().Contains(keyword.ToLower())) ||
                (p.Taskname != null &&
                 p.Taskname.ToLower().Contains(keyword.ToLower())) ||
                (p.Goaltitle != null &&
                 p.Goaltitle.ToLower().Contains(keyword.ToLower())))
            .OrderBy(p => p.Lastname)
            .ToListAsync();

        return list.Select(CarePatientsummaryMapper.ToResponseDto);
    }
}