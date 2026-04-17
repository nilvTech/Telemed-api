// Mappers/SmartgoalMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class SmartgoalMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    // Human readable measurement type labels
    private static string GetMeasurementtypeLabel(string? type)
    {
        return type?.ToUpper() switch
        {
            "BP_SYSTOLIC" => "Blood Pressure (Systolic)",
            "BP_DIASTOLIC" => "Blood Pressure (Diastolic)",
            "GLUCOSE" => "Blood Glucose",
            "HBA1C" => "HbA1c (Glycated Hemoglobin)",
            "SPO2" => "Oxygen Saturation (SpO2)",
            "HEART_RATE" => "Heart Rate",
            "WEIGHT" => "Body Weight",
            "BMI" => "Body Mass Index (BMI)",
            "LDL" => "LDL Cholesterol",
            "DIET" => "Diet / Nutrition",
            "EXERCISE" => "Exercise / Physical Activity",
            _ => type ?? "Unknown"
        };
    }

    // Auto calculate progress percentage
    private static decimal CalculateProgress(
        decimal? currentValue,
        decimal? targetValue,
        string? measurementType)
    {
        if (!currentValue.HasValue || !targetValue.HasValue || targetValue == 0)
            return 0;

        // For metrics where LOWER is better (BP, glucose, weight, BMI, LDL)
        var lowerIsBetter = new[]
        {
            "BP_SYSTOLIC", "BP_DIASTOLIC",
            "GLUCOSE", "HBA1C",
            "WEIGHT", "BMI", "LDL"
        };

        if (lowerIsBetter.Contains(measurementType?.ToUpper()))
        {
            // Progress = how much we have reduced toward target
            // If current <= target means goal achieved = 100%
            if (currentValue <= targetValue) return 100;

            // Assume a starting baseline — not stored so use simple ratio
            // (targetValue / currentValue) * 100
            var progress = (targetValue.Value / currentValue.Value) * 100;
            return Math.Min(Math.Round(progress, 2), 100);
        }

        // For metrics where HIGHER is better (SpO2, exercise minutes)
        var progress2 = (currentValue.Value / targetValue.Value) * 100;
        return Math.Min(Math.Round(progress2, 2), 100);
    }

    private static string GetProgressstatus(
        decimal? progress,
        DateOnly? targetDate,
        string? status)
    {
        if (status == "Completed") return "Achieved";
        if (status == "Cancelled") return "Cancelled";

        if (!progress.HasValue) return "NotStarted";

        var today = DateOnly.FromDateTime(DateTime.Today);

        if (targetDate.HasValue && targetDate.Value < today)
            return progress >= 80 ? "NearlyAchieved" : "Overdue";

        if (progress >= 100) return "Achieved";
        if (progress >= 75) return "OnTrack";
        if (progress >= 40) return "InProgress";
        return "AtRisk";
    }

    private static string? BuildProgressLabel(
        decimal? currentValue,
        decimal? targetValue,
        string? unit)
    {
        if (!currentValue.HasValue || !targetValue.HasValue) return null;
        return string.IsNullOrEmpty(unit)
            ? $"{currentValue} / {targetValue}"
            : $"{currentValue} {unit} / {targetValue} {unit}";
    }

    private static int? GetDaysRemaining(DateOnly? targetDate)
    {
        if (!targetDate.HasValue) return null;
        var today = DateOnly.FromDateTime(DateTime.Today);
        return targetDate.Value.DayNumber - today.DayNumber;
    }

    public static Smartgoal ToEntity(CreateSmartgoalDto dto)
    {
        var progress = CalculateProgress(
            dto.Currentvalue,
            dto.Targetvalue,
            dto.Measurementtype);

        return new Smartgoal
        {
            Careplanid = dto.Careplanid,
            Patientid = dto.Patientid,
            Providerinfoid = dto.Providerinfoid,
            Goaltitle = dto.Goaltitle,
            Description = dto.Description,
            Measurementtype = dto.Measurementtype,
            Targetvalue = dto.Targetvalue,
            Currentvalue = dto.Currentvalue,
            Unit = dto.Unit,
            Startdate = dto.Startdate,
            Targetdate = dto.Targetdate,
            Status = dto.Status ?? "Active",
            Progress = progress,
            Diettype = dto.Diettype,
            Exercisetype = dto.Exercisetype,
            Weeklyminutes = dto.Weeklyminutes,
            Notes = dto.Notes,
            Createdby = dto.Createdby,
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Smartgoal entity, UpdateSmartgoalDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Goaltitle))
            entity.Goaltitle = dto.Goaltitle;

        if (!string.IsNullOrEmpty(dto.Description))
            entity.Description = dto.Description;

        if (dto.Targetvalue.HasValue)
            entity.Targetvalue = dto.Targetvalue;

        if (dto.Currentvalue.HasValue)
        {
            entity.Currentvalue = dto.Currentvalue;
            entity.Progress = CalculateProgress(
                dto.Currentvalue,
                entity.Targetvalue,
                entity.Measurementtype);
        }

        if (!string.IsNullOrEmpty(dto.Unit))
            entity.Unit = dto.Unit;

        if (dto.Targetdate.HasValue)
            entity.Targetdate = dto.Targetdate;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (!string.IsNullOrEmpty(dto.Diettype))
            entity.Diettype = dto.Diettype;

        if (!string.IsNullOrEmpty(dto.Exercisetype))
            entity.Exercisetype = dto.Exercisetype;

        if (dto.Weeklyminutes.HasValue)
            entity.Weeklyminutes = dto.Weeklyminutes;

        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;

        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static void UpdateProgress(
        Smartgoal entity,
        SmartgoalProgressUpdateDto dto)
    {
        entity.Currentvalue = dto.Currentvalue;
        entity.Progress = CalculateProgress(
            dto.Currentvalue,
            entity.Targetvalue,
            entity.Measurementtype);

        // Auto complete if 100%
        if (entity.Progress >= 100)
            entity.Status = "Completed";

        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;

        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static SmartgoalResponseDto ToResponseDto(Smartgoal entity)
    {
        var daysRemaining = GetDaysRemaining(entity.Targetdate);
        var isOverdue = daysRemaining.HasValue && daysRemaining < 0 &&
                             entity.Status != "Completed";

        return new SmartgoalResponseDto
        {
            Smartgoalid = entity.Smartgoalid,

            // Careplan
            Careplanid = entity.Careplanid,
            CareplanStatus = entity.Careplan?.Status,

            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                                     ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                                       .Replace("  ", " ").Trim()
                                     : null,
            Mrn = entity.Patient?.Mrn,

            // Provider
            Providerinfoid = entity.Providerinfoid,
            Providername = entity.Providerinfo != null
                                     ? $"{entity.Providerinfo.Firstname} {entity.Providerinfo.Lastname}"
                                       .Trim()
                                     : null,

            // Goal
            Goaltitle = entity.Goaltitle,
            Description = entity.Description,
            Measurementtype = entity.Measurementtype,
            MeasurementtypeLabel = GetMeasurementtypeLabel(entity.Measurementtype),

            // Values
            Targetvalue = entity.Targetvalue,
            Currentvalue = entity.Currentvalue,
            Unit = entity.Unit,
            Progress = entity.Progress,
            ProgressLabel = BuildProgressLabel(
                                         entity.Currentvalue,
                                         entity.Targetvalue,
                                         entity.Unit),
            Progressstatus = GetProgressstatus(
                                         entity.Progress,
                                         entity.Targetdate,
                                         entity.Status),

            // Dates
            Startdate = entity.Startdate,
            Targetdate = entity.Targetdate,
            DaysRemaining = daysRemaining,
            IsOverdue = isOverdue,

            // Status
            Status = entity.Status,

            // Lifestyle
            Diettype = entity.Diettype,
            Exercisetype = entity.Exercisetype,
            Weeklyminutes = entity.Weeklyminutes,

            Notes = entity.Notes,
            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat
        };
    }
}