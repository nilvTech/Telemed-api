// Services/PatientDashboardService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PatientDashboardService : IPatientDashboardService
{
    private readonly TelemedDbContext _context;

    public PatientDashboardService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientDashboardResponseDto>> GetAllAsync()
    {
        var list = await _context.PatientDashboards
            .OrderBy(p => p.Patientname)
            .ToListAsync();

        return list.Select(PatientDashboardMapper.ToResponseDto);
    }

    public async Task<PatientDashboardResponseDto?> GetByPatientIdAsync(
        long patientId)
    {
        var entity = await _context.PatientDashboards
            .FirstOrDefaultAsync(p => p.Patientid == patientId);

        if (entity == null) return null;
        return PatientDashboardMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<PatientDashboardResponseDto>> GetWithAppointmentsTodayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.PatientDashboards
            .Where(p => p.Appointmentdate == today)
            .OrderBy(p => p.Appointmentstarttime)
            .ToListAsync();

        return list.Select(PatientDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientDashboardResponseDto>> GetWithPendingClaimsAsync()
    {
        var list = await _context.PatientDashboards
            .Where(p => p.Billingstatus != null &&
                        (p.Billingstatus == "Pending" ||
                         p.Billingstatus == "Submitted"))
            .OrderBy(p => p.Patientname)
            .ToListAsync();

        return list.Select(PatientDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientDashboardResponseDto>> GetWithUnreadNotificationsAsync()
    {
        var list = await _context.PatientDashboards
            .Where(p => p.Notificationid != null)
            .OrderByDescending(p => p.Notificationdate)
            .ToListAsync();

        return list.Select(PatientDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientDashboardResponseDto>> GetWithActiveVideoSessionsAsync()
    {
        var list = await _context.PatientDashboards
            .Where(p => p.Videosessionid != null &&
                        p.Callstatus == "InProgress")
            .OrderByDescending(p => p.Starttime)
            .ToListAsync();

        return list.Select(PatientDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientDashboardResponseDto>> SearchAsync(
        string keyword)
    {
        var list = await _context.PatientDashboards
            .Where(p =>
                (p.Patientname != null &&
                 p.Patientname.ToLower().Contains(keyword.ToLower())) ||
                (p.Mrn != null &&
                 p.Mrn.ToLower().Contains(keyword.ToLower())) ||
                (p.Doctorname != null &&
                 p.Doctorname.ToLower().Contains(keyword.ToLower())) ||
                (p.Claimnumber != null &&
                 p.Claimnumber.ToLower().Contains(keyword.ToLower())))
            .OrderBy(p => p.Patientname)
            .ToListAsync();

        return list.Select(PatientDashboardMapper.ToResponseDto);
    }
}