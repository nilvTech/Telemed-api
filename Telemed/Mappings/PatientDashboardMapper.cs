// Mappers/PatientDashboardMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class PatientDashboardMapper
{
    private static int? CalculateAge(DateOnly? dob)
    {
        if (!dob.HasValue) return null;
        var today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - dob.Value.Year;
        if (dob.Value > today.AddYears(-age)) age--;
        return age;
    }

    private static string? GetBpStatus(int? sys, int? dia)
    {
        if (!sys.HasValue || !dia.HasValue) return null;
        if (sys >= 180 || dia >= 120) return "HypertensiveCrisis";
        if (sys >= 140 || dia >= 90) return "HighStage2";
        if (sys >= 130 || dia >= 80) return "HighStage1";
        if (sys >= 120 && dia < 80) return "Elevated";
        return "Normal";
    }

    private static string? GetHeartrateStatus(int? hr)
    {
        if (!hr.HasValue) return null;
        if (hr < 60) return "Low";
        if (hr > 100) return "High";
        return "Normal";
    }

    private static string? GetSpo2Status(int? spo2)
    {
        if (!spo2.HasValue) return null;
        if (spo2 < 90) return "Critical";
        if (spo2 < 95) return "Low";
        return "Normal";
    }

    public static PatientDashboardResponseDto ToResponseDto(
        PatientDashboard entity)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        string? bp = entity.Systolic.HasValue && entity.Diastolic.HasValue
            ? $"{entity.Systolic}/{entity.Diastolic}"
            : null;

        decimal? balance = entity.Totalamount.HasValue
            ? entity.Totalamount - (entity.Paidamount ?? 0)
            : null;

        return new PatientDashboardResponseDto
        {
            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patientname,
            Gender = entity.Gender,
            Dateofbirth = entity.Dateofbirth,
            Age = CalculateAge(entity.Dateofbirth),
            Mrn = entity.Mrn,

            // Appointment
            Appointmentid = entity.Appointmentid,
            Appointmentdate = entity.Appointmentdate,
            Appointmentstarttime = entity.Appointmentstarttime,
            Appointmentstatus = entity.Appointmentstatus,
            Visittype = entity.Visittype,
            HasAppointment = entity.Appointmentid.HasValue,
            IsAppointmentToday = entity.Appointmentdate.HasValue &&
                                    entity.Appointmentdate.Value == today,

            // Provider
            Providerid = entity.Providerid,
            Doctorname = entity.Doctorname,
            Speciality = entity.Speciality,

            // Video Session
            Videosessionid = entity.Videosessionid,
            Callstatus = entity.Callstatus,
            Starttime = entity.Starttime,
            Endtime = entity.Endtime,
            Recordingurl = entity.Recordingurl,
            HasVideoSession = entity.Videosessionid.HasValue,

            // Health Summary
            Systolic = entity.Systolic,
            Diastolic = entity.Diastolic,
            Bloodpressure = bp,
            BpStatus = GetBpStatus(entity.Systolic, entity.Diastolic),
            Heartrate = entity.Heartrate,
            HeartrateStatus = GetHeartrateStatus(entity.Heartrate),
            Spo2 = entity.Spo2,
            Spo2Status = GetSpo2Status(entity.Spo2),
            Glucose = entity.Glucose,
            Temperature = entity.Temperature,
            Weight = entity.Weight,
            Readingdate = entity.Readingdate,
            HasRpmData = entity.Readingdate.HasValue,

            // Billing
            Claimid = entity.Claimid,
            Claimnumber = entity.Claimnumber,
            Payer = entity.Payer,
            Totalamount = entity.Totalamount,
            Paidamount = entity.Paidamount,
            Balancedue = balance,
            Billingstatus = entity.Billingstatus,
            HasClaim = entity.Claimid.HasValue,

            // Notifications
            Notificationid = entity.Notificationid,
            Notificationmessage = entity.Notificationmessage,
            Notificationdate = entity.Notificationdate,
            HasNotification = entity.Notificationid.HasValue,

            // Encounter
            Encounterid = entity.Encounterid,
            Encounterdate = entity.Encounterdate,
            Encounternotes = entity.Encounternotes,
            HasEncounter = entity.Encounterid.HasValue
        };
    }
}