// Services/PatientAppointmentDashboardService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PatientAppointmentDashboardService
    : IPatientAppointmentDashboardService
{
    private readonly TelemedDbContext _context;

    public PatientAppointmentDashboardService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetAllAsync()
    {
        var list = await _context.PatientAppointmentDashboards
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetByPatientIdAsync(long patientId)
    {
        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Patientid == patientId)
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<PatientAppointmentDashboardResponseDto?>
        GetByAppointmentIdAsync(long appointmentId)
    {
        var entity = await _context.PatientAppointmentDashboards
            .FirstOrDefaultAsync(a => a.Appointmentid == appointmentId);

        if (entity == null) return null;
        return PatientAppointmentDashboardMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetByProviderIdAsync(long providerId)
    {
        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Providerid == providerId)
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetTodayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Appointmentdate == today)
            .OrderBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetUpcomingAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Appointmentdate > today &&
                        (a.Appointmentstatus == "Booked" ||
                         a.Appointmentstatus == "Confirmed"))
            .OrderBy(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetPastAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Appointmentdate < today)
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetByStatusAsync(string status)
    {
        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Appointmentstatus != null &&
                        a.Appointmentstatus.ToLower() == status.ToLower())
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetByVisitmodeAsync(string visitmode)
    {
        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Visitmode != null &&
                        a.Visitmode.ToLower() == visitmode.ToLower())
            .OrderByDescending(a => a.Appointmentdate)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetTelemedicineAsync()
    {
        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Visitmode != null &&
                        a.Visitmode.ToLower() == "telemedicine" &&
                        a.Canjoincall == true)
            .OrderByDescending(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetWithRecordingsAsync()
    {
        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Recordingurl != null)
            .OrderByDescending(a => a.Videostarttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        GetByDateRangeAsync(DateOnly from, DateOnly to)
    {
        var list = await _context.PatientAppointmentDashboards
            .Where(a => a.Appointmentdate.HasValue &&
                        a.Appointmentdate >= from &&
                        a.Appointmentdate <= to)
            .OrderBy(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }

    public async Task<IEnumerable<PatientAppointmentDashboardResponseDto>>
        SearchAsync(string keyword)
    {
        var list = await _context.PatientAppointmentDashboards
            .Where(a =>
                (a.Patientname != null &&
                 a.Patientname.ToLower().Contains(keyword.ToLower())) ||
                (a.Mrn != null &&
                 a.Mrn.ToLower().Contains(keyword.ToLower())) ||
                (a.Providername != null &&
                 a.Providername.ToLower().Contains(keyword.ToLower())) ||
                (a.Visitreason != null &&
                 a.Visitreason.ToLower().Contains(keyword.ToLower())) ||
                (a.Meetingid != null &&
                 a.Meetingid.ToLower().Contains(keyword.ToLower())))
            .OrderByDescending(a => a.Appointmentdate)
            .ToListAsync();

        return list.Select(
            PatientAppointmentDashboardMapper.ToResponseDto);
    }
}