// Mappers/CarePatientsummaryMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class CarePatientsummaryMapper
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

    private static string GetTaskDuedateStatus(DateOnly? duedate)
    {
        if (!duedate.HasValue) return "Unknown";
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (duedate < today) return "Overdue";
        if (duedate == today) return "Today";
        return "Upcoming";
    }

    public static CarePatientsummaryResponseDto ToResponseDto(
        Carepatientsummary entity)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var conditionList = !string.IsNullOrEmpty(entity.Conditions)
            ? entity.Conditions
                .Split(',', StringSplitOptions.TrimEntries |
                            StringSplitOptions.RemoveEmptyEntries)
                .ToList()
            : new List<string>();

        string? bp = entity.Systolic.HasValue && entity.Diastolic.HasValue
            ? $"{entity.Systolic}/{entity.Diastolic}"
            : null;

        string? progressLabel = entity.Progress.HasValue
            ? $"{entity.Progress:F0}%"
            : null;

        return new CarePatientsummaryResponseDto
        {
            // Patient
            Patientid = entity.Patientid,
            Firstname = entity.Firstname,
            Lastname = entity.Lastname,
            Fullname = $"{entity.Firstname} {entity.Lastname}".Trim(),
            Gender = entity.Gender,
            Dateofbirth = entity.Dateofbirth,
            Age = CalculateAge(entity.Dateofbirth),

            // Conditions
            Conditions = entity.Conditions,
            Conditioncount = conditionList.Count,
            Conditionlist = conditionList,

            // RPM
            LatestReadingdate = entity.LatestReadingdate,
            Systolic = entity.Systolic,
            Diastolic = entity.Diastolic,
            Bloodpressure = bp,
            BpStatus = GetBpStatus(entity.Systolic, entity.Diastolic),
            Heartrate = entity.Heartrate,
            HeartrateStatus = GetHeartrateStatus(entity.Heartrate),
            Spo2 = entity.Spo2,
            Spo2Status = GetSpo2Status(entity.Spo2),
            Glucose = entity.Glucose,
            Weight = entity.Weight,
            HasRpmData = entity.LatestReadingdate.HasValue,

            // Alert
            Alerttype = entity.Alerttype,
            HasAlert = !string.IsNullOrEmpty(entity.Alerttype),

            // Followup
            Followuptype = entity.Followuptype,
            Followupdate = entity.Followupdate,
            FollowupStatus = entity.FollowupStatus,
            IsFollowupOverdue = entity.Followupdate.HasValue &&
                                 entity.Followupdate < today &&
                                 entity.FollowupStatus == "Scheduled",

            // Task
            Taskname = entity.Taskname,
            Duedate = entity.Duedate,
            TaskStatus = entity.TaskStatus,
            IsTaskOverdue = entity.Duedate.HasValue &&
                                 entity.Duedate < today &&
                                 entity.TaskStatus != "Completed" &&
                                 entity.TaskStatus != "Cancelled",
            TaskDuedateStatus = GetTaskDuedateStatus(entity.Duedate),

            // Careplan
            CareplanStatus = entity.CareplanStatus,
            HasActiveCareplan = entity.CareplanStatus == "Active",

            // Smartgoal
            Goaltitle = entity.Goaltitle,
            Progress = entity.Progress,
            Progresslabel = progressLabel
        };
    }
}