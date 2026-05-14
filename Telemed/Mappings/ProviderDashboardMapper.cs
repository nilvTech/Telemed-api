// Mappers/ProviderDashboardMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ProviderDashboardMapper
{
    // "2 hours ago" / "5 min ago" / "Yesterday"
    public static string GetTimeAgo(DateTime? dt)
    {
        if (!dt.HasValue) return string.Empty;

        var diff = DateTime.UtcNow - dt.Value;

        if (diff.TotalMinutes < 1) return "Just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} min ago";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} hour ago";
        if (diff.TotalDays < 2) return "Yesterday";
        return $"{(int)diff.TotalDays} days ago";
    }

    // "Today" / "Monday" / "Saturday"
    public static string GetDayLabel(DateOnly? date)
    {
        if (!date.HasValue) return string.Empty;

        var today = DateOnly.FromDateTime(DateTime.Today);
        if (date.Value == today) return "Today";
        if (date.Value == today.AddDays(1)) return "Tomorrow";

        return date.Value.ToString("dddd"); // Monday, Tuesday etc
    }

    // "09:00 - 09:45AM"
    public static string GetTimeRange(TimeOnly? start, TimeOnly? end)
    {
        if (!start.HasValue) return string.Empty;
        var startStr = start.Value.ToString("HH:mm");
        var endStr = end.HasValue
            ? end.Value.ToString("HH:mmtt")
            : string.Empty;
        return $"{startStr} - {endStr}";
    }

    public static ProviderDashboardAppointmentDto ToAppointmentDto(
        Appointment entity)
    {
        var patientName = entity.Patient != null
            ? $"{entity.Patient.Firstname} {entity.Patient.Lastname}".Trim()
            : null;

        return new ProviderDashboardAppointmentDto
        {
            Appointmentid = entity.Appointmentid,
            Patientname = patientName,
            Visitreason = entity.Visitreason,
            Daylabel = GetDayLabel(entity.Appointmentdate),
            Timerange = GetTimeRange(entity.Starttime, entity.Endtime),
            Status = entity.Status,
            Visitmode = entity.Visitmode,
            Appointmentdate = entity.Appointmentdate,
            Starttime = entity.Starttime,
            Endtime = entity.Endtime
        };
    }

    public static ProviderDashboardNotificationDto ToNotificationDto(
        Notification entity)
    {
        return new ProviderDashboardNotificationDto
        {
            Notificationid = entity.Notificationid,
            Title = entity.Title,
            Message = entity.Message,
            Timeago = GetTimeAgo(entity.Createdat),
            Createdat = entity.Createdat,
            Isread = entity.Isread
        };
    }

    public static ProviderDashboardConsultationDto ToConsultationDto(
        Consultation entity)
    {
        var patientName = entity.Patient != null
            ? $"{entity.Patient.Firstname} {entity.Patient.Lastname}".Trim()
            : null;

        // Calculate waiting minutes from starttime
        int? waitingMinutes = null;
        if (entity.Starttime.HasValue)
        {
            var diff = DateTime.UtcNow - entity.Starttime.Value;
            waitingMinutes = (int)diff.TotalMinutes;
        }

        return new ProviderDashboardConsultationDto
        {
            Consultationid = entity.Consultationid,
            Patientname = patientName,
            Mrn = entity.Patient?.Mrn,
            Waitingminutes = waitingMinutes,
            Waitinglabel = waitingMinutes.HasValue
                              ? $"Waiting {waitingMinutes} min"
                              : "Waiting",
            Status = entity.Status,
            Starttime = entity.Starttime
        };
    }

    public static ProviderDashboardAlertDto ToAlertDto(
        Patientalert entity)
    {
        var patientName = entity.Patient != null
            ? $"{entity.Patient.Firstname} {entity.Patient.Lastname}".Trim()
            : null;

        return new ProviderDashboardAlertDto
        {
            Alertid = entity.Alertid,
            Patientname = patientName,
            Alertmessage = entity.Alertmessage,
            Alerttype = entity.Alerttype,
            Severity = entity.Severity,
            Isacknowledged = entity.Isacknowledged,
            Createddate = entity.Createddate
        };
    }
}