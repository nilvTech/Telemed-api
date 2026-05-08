// Mappers/PatientAppointmentDashboardMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class PatientAppointmentDashboardMapper
{
    private static string? FormatDuration(int? seconds)
    {
        if (!seconds.HasValue || seconds <= 0) return null;
        if (seconds < 60) return $"{seconds} sec";
        var minutes = seconds / 60;
        var secs = seconds % 60;
        return secs > 0
            ? $"{minutes} min {secs} sec"
            : $"{minutes} min";
    }

    private static int? GetDaysUntilAppointment(DateOnly? appointmentdate)
    {
        if (!appointmentdate.HasValue) return null;
        var today = DateOnly.FromDateTime(DateTime.Today);
        return appointmentdate.Value.DayNumber - today.DayNumber;
    }

    public static PatientAppointmentDashboardResponseDto ToResponseDto(
        PatientAppointmentDashboard entity)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var daysUntil = GetDaysUntilAppointment(entity.Appointmentdate);
        var isToday = entity.Appointmentdate.HasValue &&
                             entity.Appointmentdate.Value == today;
        var isUpcoming = daysUntil.HasValue && daysUntil > 0;
        var isPast = daysUntil.HasValue && daysUntil < 0;

        return new PatientAppointmentDashboardResponseDto
        {
            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patientname,
            Gender = entity.Gender,
            Mrn = entity.Mrn,
            Age = entity.Age,

            // Appointment
            Appointmentid = entity.Appointmentid,
            Appointmentdate = entity.Appointmentdate,
            Starttime = entity.Starttime,
            Endtime = entity.Endtime,
            Visittype = entity.Visittype,
            Visitmode = entity.Visitmode,
            Visitreason = entity.Visitreason,
            Appointmentstatus = entity.Appointmentstatus,
            Ispaid = entity.Ispaid,
            Paymentid = entity.Paymentid,
            IsToday = isToday,
            IsUpcoming = isUpcoming,
            IsPast = isPast,
            DaysUntilAppointment = daysUntil,

            // Provider
            Providerid = entity.Providerid,
            Providername = entity.Providername,
            Speciality = entity.Speciality,

            // Video Session
            Videosessionid = entity.Videosessionid,
            Videocallstatus = entity.Videocallstatus,
            Videostarttime = entity.Videostarttime,
            Videoendtime = entity.Videoendtime,
            Recordingurl = entity.Recordingurl,
            Videoname = entity.Videoname,
            Durationseconds = entity.Durationseconds,
            Durationformatted = FormatDuration(entity.Durationseconds),
            HasVideoSession = entity.Videosessionid.HasValue,
            HasRecording = !string.IsNullOrEmpty(entity.Recordingurl),

            // Actions
            Canjoincall = entity.Canjoincall,
            Canreschedule = entity.Canreschedule,
            Cancancel = entity.Cancancel,
            Meetinglink = entity.Meetinglink,
            Meetingid = entity.Meetingid,

            // Audit
            Createddate = entity.Createddate,
            Updateddate = entity.Updateddate
        };
    }
}