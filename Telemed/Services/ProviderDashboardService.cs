// Services/ProviderDashboardService.cs

using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ProviderDashboardService : IProviderDashboardService
{
    private readonly TelemedDbContext _context;

    public ProviderDashboardService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<ProviderDashboardResponseDto?> GetDashboardAsync(
        long providerinfoid)
    {
        // Validate provider exists
        var provider = await _context.Providerinfos
            .FirstOrDefaultAsync(p => p.Providerinfoid == providerinfoid);

        if (provider == null)
            return null;

        // Provider display name
        var displayName = !string.IsNullOrEmpty(provider.Displayname)
            ? provider.Displayname
            : $"Dr. {provider.Firstname} {provider.Lastname}".Trim();

        // Sequential async calls (DbContext thread-safe nahi hota)
        var stats = await GetStatsAsync(providerinfoid);

        var appointments = await GetTodaysAppointmentsAsync(providerinfoid);

        var notifications = await GetNotificationsAsync(providerinfoid);

        var consultations = await GetPendingConsultationsAsync(providerinfoid);

        var alerts = await GetPatientAlertsAsync(providerinfoid);

        return new ProviderDashboardResponseDto
        {
            Providerinfoid = providerinfoid,
            Providername = displayName,
            WelcomeMessage = $"Welcome back, {displayName}!",
            Stats = stats,
            TodaysAppointments = appointments,
            Notifications = notifications,
            PendingConsultations = consultations,
            PatientAlerts = alerts
        };
    }

    public async Task<ProviderDashboardStatsDto> GetStatsAsync(
        long providerinfoid)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        // Get all today's appointments
        var todayAppointments = await _context.Appointments
            .Where(a =>
                a.Providerid == providerinfoid &&
                a.Appointmentdate == today &&
                a.Isactive == true)
            .ToListAsync();

        // Get all appointments
        var allAppointments = await _context.Appointments
            .Where(a =>
                a.Providerid == providerinfoid &&
                a.Isactive == true)
            .ToListAsync();

        return new ProviderDashboardStatsDto
        {
            PatientsToday = todayAppointments
                .Select(a => a.Patientid)
                .Distinct()
                .Count(),

            AppointmentsCompleted = allAppointments
                .Count(a => a.Status == "Completed"),

            AppointmentsPending = allAppointments
                .Count(a =>
                    a.Status == "Booked" ||
                    a.Status == "Confirmed"),

            TotalAppointments = allAppointments.Count
        };
    }

    public async Task<List<ProviderDashboardAppointmentDto>>
        GetTodaysAppointmentsAsync(long providerinfoid)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        // Today + next 7 days
        var upcomingDate = today.AddDays(7);

        var list = await _context.Appointments
            .Include(a => a.Patient)
            .Where(a =>
                a.Providerid == providerinfoid &&
                a.Appointmentdate >= today &&
                a.Appointmentdate <= upcomingDate &&
                a.Isactive == true &&
                a.Status != "Cancelled")
            .OrderBy(a => a.Appointmentdate)
            .ThenBy(a => a.Starttime)
            .Take(10)
            .ToListAsync();

        return list
            .Select(ProviderDashboardMapper.ToAppointmentDto)
            .ToList();
    }

    public async Task<List<ProviderDashboardNotificationDto>>
        GetNotificationsAsync(long providerinfoid)
    {
        var list = await _context.Notifications
            .Where(n =>
                n.Userid == (int)providerinfoid &&
                n.Usertype == "Providerinfo")
            .OrderByDescending(n => n.Createdat)
            .Take(5)
            .ToListAsync();

        return list
            .Select(ProviderDashboardMapper.ToNotificationDto)
            .ToList();
    }

    public async Task<List<ProviderDashboardConsultationDto>>
        GetPendingConsultationsAsync(long providerinfoid)
    {
        var list = await _context.Consultations
            .Include(c => c.Patient)
            .Where(c =>
                c.Providerid == providerinfoid &&
                c.Status == "Pending" &&
                c.Isactive == true)
            .OrderBy(c => c.Starttime)
            .Take(10)
            .ToListAsync();

        return list
            .Select(ProviderDashboardMapper.ToConsultationDto)
            .ToList();
    }

    public async Task<List<ProviderDashboardAlertDto>>
        GetPatientAlertsAsync(long providerinfoid)
    {
        var patientIds = await _context.Appointments
            .Where(a =>
                a.Providerid == providerinfoid &&
                a.Isactive == true)
            .Select(a => a.Patientid)
            .Distinct()
            .ToListAsync();

        var list = await _context.Patientalerts
            .Include(a => a.Patient)
            .Where(a =>
                patientIds.Contains(a.Patientid) &&
                a.Isacknowledged == false &&
                a.Isactive == true)
            .OrderByDescending(a => a.Createddate)
            .Take(10)
            .ToListAsync();

        return list
            .Select(ProviderDashboardMapper.ToAlertDto)
            .ToList();
    }
}