// Mappers/RpmmonitoringMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class RpmmonitoringMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    // ===== Blood Pressure Status =====
    private static string? GetBpStatus(int? systolic, int? diastolic)
    {
        if (!systolic.HasValue || !diastolic.HasValue) return null;
        if (systolic >= 180 || diastolic >= 120) return "HypertensiveCrisis";
        if (systolic >= 140 || diastolic >= 90) return "HighStage2";
        if (systolic >= 130 || diastolic >= 80) return "HighStage1";
        if (systolic >= 120 && diastolic < 80) return "Elevated";
        return "Normal";
    }

    // ===== Heart Rate Status =====
    private static string? GetHeartrateStatus(int? hr)
    {
        if (!hr.HasValue) return null;
        if (hr < 60) return "Low";
        if (hr > 100) return "High";
        return "Normal";
    }

    // ===== SpO2 Status =====
    private static string? GetSpo2Status(int? spo2)
    {
        if (!spo2.HasValue) return null;
        if (spo2 < 90) return "Critical";
        if (spo2 < 95) return "Low";
        return "Normal";
    }

    // ===== Glucose Status (mg/dL) =====
    private static string? GetGlucoseStatus(int? glucose, string? unit)
    {
        if (!glucose.HasValue) return null;
        // Fasting glucose reference
        if (unit?.ToLower() == "mmol/l")
        {
            if (glucose < 4) return "Low";
            if (glucose > 7) return "High";
            return "Normal";
        }
        if (glucose < 70) return "Low";
        if (glucose > 126) return "High";
        return "Normal";
    }

    // ===== Temperature Status (F) =====
    private static string? GetTemperatureStatus(decimal? temp, string? unit)
    {
        if (!temp.HasValue) return null;
        decimal t = temp.Value;
        // Convert C to F for comparison
        if (unit?.ToLower() == "c")
            t = (t * 9 / 5) + 32;
        if (t < 96.8m) return "Hypothermia";
        if (t > 100.4m) return "Fever";
        if (t > 103.0m) return "HighFever";
        return "Normal";
    }

    // ===== BMI Calculation =====
    private static (decimal? bmi, string? status) CalculateBmi(
        decimal? weight, string? weightUnit,
        decimal? height, string? heightUnit)
    {
        if (!weight.HasValue || !height.HasValue || height == 0)
            return (null, null);

        // Convert to kg and meters
        decimal weightKg = weightUnit?.ToLower() == "lbs"
            ? weight.Value * 0.453592m
            : weight.Value;

        decimal heightM = heightUnit?.ToLower() == "inch"
            ? height.Value * 0.0254m
            : height.Value / 100;

        if (heightM == 0) return (null, null);

        decimal bmi = Math.Round(weightKg / (heightM * heightM), 1);

        string status = bmi switch
        {
            < 18.5m => "Underweight",
            < 25.0m => "Normal",
            < 30.0m => "Overweight",
            _ => "Obese"
        };

        return (bmi, status);
    }

    public static Rpmmonitoring ToEntity(CreateRpmmonitoringDto dto)
    {
        return new Rpmmonitoring
        {
            Patientid = dto.Patientid,
            Readingdate = ToUnspecified(dto.Readingdate),
            Systolic = dto.Systolic,
            Diastolic = dto.Diastolic,
            Heartrate = dto.Heartrate,
            Spo2 = dto.Spo2,
            Respiratoryrate = dto.Respiratoryrate,
            Glucose = dto.Glucose,
            Glucoseunit = dto.Glucoseunit ?? "mg/dL",
            Temperature = dto.Temperature,
            Temperatureunit = dto.Temperatureunit ?? "F",
            Weight = dto.Weight,
            Weightunit = dto.Weightunit ?? "lbs",
            Height = dto.Height,
            Heightunit = dto.Heightunit ?? "inch",
            Devicetype = dto.Devicetype,
            Deviceid = dto.Deviceid,
            Sourcedata = dto.Sourcedata,
            Isauto = dto.Isauto ?? false,
            Isreviewed = false,
            Note = dto.Note,
            Createdby = dto.Createdby,
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Rpmmonitoring entity, UpdateRpmmonitoringDto dto)
    {
        if (dto.Systolic.HasValue) entity.Systolic = dto.Systolic;
        if (dto.Diastolic.HasValue) entity.Diastolic = dto.Diastolic;
        if (dto.Heartrate.HasValue) entity.Heartrate = dto.Heartrate;
        if (dto.Spo2.HasValue) entity.Spo2 = dto.Spo2;
        if (dto.Respiratoryrate.HasValue) entity.Respiratoryrate = dto.Respiratoryrate;
        if (dto.Glucose.HasValue) entity.Glucose = dto.Glucose;
        if (dto.Temperature.HasValue) entity.Temperature = dto.Temperature;
        if (dto.Weight.HasValue) entity.Weight = dto.Weight;
        if (dto.Height.HasValue) entity.Height = dto.Height;

        if (!string.IsNullOrEmpty(dto.Glucoseunit)) entity.Glucoseunit = dto.Glucoseunit;
        if (!string.IsNullOrEmpty(dto.Temperatureunit)) entity.Temperatureunit = dto.Temperatureunit;
        if (!string.IsNullOrEmpty(dto.Weightunit)) entity.Weightunit = dto.Weightunit;
        if (!string.IsNullOrEmpty(dto.Heightunit)) entity.Heightunit = dto.Heightunit;
        if (!string.IsNullOrEmpty(dto.Devicetype)) entity.Devicetype = dto.Devicetype;
        if (!string.IsNullOrEmpty(dto.Deviceid)) entity.Deviceid = dto.Deviceid;
        if (!string.IsNullOrEmpty(dto.Note)) entity.Note = dto.Note;

        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static RpmmonitoringResponseDto ToResponseDto(Rpmmonitoring entity)
    {
        var (bmi, bmiStatus) = CalculateBmi(
            entity.Weight, entity.Weightunit,
            entity.Height, entity.Heightunit);

        string? bp = entity.Systolic.HasValue && entity.Diastolic.HasValue
            ? $"{entity.Systolic}/{entity.Diastolic}"
            : null;

        return new RpmmonitoringResponseDto
        {
            Rpmmonitoringid = entity.Rpmmonitoringid,

            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                                ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                                  .Replace("  ", " ").Trim()
                                : null,
            Mrn = entity.Patient?.Mrn,
            Gender = entity.Patient?.Gender,

            Readingdate = entity.Readingdate,

            // Blood Pressure
            Systolic = entity.Systolic,
            Diastolic = entity.Diastolic,
            Bloodpressure = bp,
            Bpstatus = GetBpStatus(entity.Systolic, entity.Diastolic),

            // Vitals
            Heartrate = entity.Heartrate,
            Heartratestatus = GetHeartrateStatus(entity.Heartrate),
            Spo2 = entity.Spo2,
            Spo2status = GetSpo2Status(entity.Spo2),
            Respiratoryrate = entity.Respiratoryrate,

            // Glucose
            Glucose = entity.Glucose,
            Glucoseunit = entity.Glucoseunit,
            Glucosestatus = GetGlucoseStatus(entity.Glucose, entity.Glucoseunit),

            // Temperature
            Temperature = entity.Temperature,
            Temperatureunit = entity.Temperatureunit,
            Temperaturestatus = GetTemperatureStatus(
                entity.Temperature, entity.Temperatureunit),

            // Weight & Height
            Weight = entity.Weight,
            Weightunit = entity.Weightunit,
            Height = entity.Height,
            Heightunit = entity.Heightunit,
            Bmi = bmi,
            Bmistatus = bmiStatus,

            // Device
            Devicetype = entity.Devicetype,
            Deviceid = entity.Deviceid,
            Sourcedata = entity.Sourcedata,
            Isauto = entity.Isauto,

            // Review
            Isreviewed = entity.Isreviewed,
            Reviewedat = entity.Reviewedat,
            Reviewedby = entity.Reviewedby,
            Reviewedbyname = entity.ReviewedByProvider != null
                                ? $"{entity.ReviewedByProvider.Firstname} {entity.ReviewedByProvider.Lastname}"
                                  .Trim()
                                : null,

            Note = entity.Note,
            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat
        };
    }
}