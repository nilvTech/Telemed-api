// Mappers/CareplanMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class CareplanMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    // CCM = Chronic Care Management — 20min/month threshold
    private static string GetCcmStatus(int? minutes)
    {
        if (!minutes.HasValue) return "NotTracked";
        return minutes.Value >= 20 ? "Met" : "NotMet";
    }

    private static int? GetDaysUntilNextReview(DateOnly? nextReviewDate)
    {
        if (!nextReviewDate.HasValue) return null;
        var today = DateOnly.FromDateTime(DateTime.Today);
        return nextReviewDate.Value.DayNumber - today.DayNumber;
    }

    private static bool GetIsOverdueForReview(DateOnly? nextReviewDate)
    {
        if (!nextReviewDate.HasValue) return false;
        var today = DateOnly.FromDateTime(DateTime.Today);
        return nextReviewDate.Value < today;
    }

    public static Careplan ToEntity(CreateCareplanDto dto)
    {
        return new Careplan
        {
            Patientid = dto.Patientid,
            Providerinfoid = dto.Providerinfoid,
            Startdate = dto.Startdate,
            Enddate = dto.Enddate,
            Status = dto.Status ?? "Active",
            Risklevel = dto.Risklevel ?? "Medium",
            Ccmminutes = dto.Ccmminutes ?? 0,
            Bpsystolictarget = dto.Bpsystolictarget,
            Bpdiastolictarget = dto.Bpdiastolictarget,
            Hba1ctarget = dto.Hba1ctarget,
            Glucosetargetmin = dto.Glucosetargetmin,
            Glucosetargetmax = dto.Glucosetargetmax,
            Spo2target = dto.Spo2target,
            Heartratetargetmin = dto.Heartratetargetmin,
            Heartratetargetmax = dto.Heartratetargetmax,
            Weighttarget = dto.Weighttarget,
            Bmitarget = dto.Bmitarget,
            Ldltarget = dto.Ldltarget,
            Problems = dto.Problems,
            Goals = dto.Goals,
            Interventions = dto.Interventions,
            Medications = dto.Medications,
            Allergies = dto.Allergies,
            Nextreviewdate = dto.Nextreviewdate,
            Lastreviewdate = dto.Lastreviewdate,
            Reviewfrequency = dto.Reviewfrequency,
            Createdby = dto.Createdby,
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Careplan entity, UpdateCareplanDto dto)
    {
        if (dto.Providerinfoid.HasValue)
            entity.Providerinfoid = dto.Providerinfoid;

        if (dto.Enddate.HasValue)
            entity.Enddate = dto.Enddate;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (!string.IsNullOrEmpty(dto.Risklevel))
            entity.Risklevel = dto.Risklevel;

        if (dto.Ccmminutes.HasValue)
            entity.Ccmminutes = dto.Ccmminutes;

        if (dto.Bpsystolictarget.HasValue)
            entity.Bpsystolictarget = dto.Bpsystolictarget;

        if (dto.Bpdiastolictarget.HasValue)
            entity.Bpdiastolictarget = dto.Bpdiastolictarget;

        if (dto.Hba1ctarget.HasValue)
            entity.Hba1ctarget = dto.Hba1ctarget;

        if (dto.Glucosetargetmin.HasValue)
            entity.Glucosetargetmin = dto.Glucosetargetmin;

        if (dto.Glucosetargetmax.HasValue)
            entity.Glucosetargetmax = dto.Glucosetargetmax;

        if (dto.Spo2target.HasValue)
            entity.Spo2target = dto.Spo2target;

        if (dto.Heartratetargetmin.HasValue)
            entity.Heartratetargetmin = dto.Heartratetargetmin;

        if (dto.Heartratetargetmax.HasValue)
            entity.Heartratetargetmax = dto.Heartratetargetmax;

        if (dto.Weighttarget.HasValue)
            entity.Weighttarget = dto.Weighttarget;

        if (dto.Bmitarget.HasValue)
            entity.Bmitarget = dto.Bmitarget;

        if (dto.Ldltarget.HasValue)
            entity.Ldltarget = dto.Ldltarget;

        if (!string.IsNullOrEmpty(dto.Problems))
            entity.Problems = dto.Problems;

        if (!string.IsNullOrEmpty(dto.Goals))
            entity.Goals = dto.Goals;

        if (!string.IsNullOrEmpty(dto.Interventions))
            entity.Interventions = dto.Interventions;

        if (!string.IsNullOrEmpty(dto.Medications))
            entity.Medications = dto.Medications;

        if (!string.IsNullOrEmpty(dto.Allergies))
            entity.Allergies = dto.Allergies;

        if (dto.Nextreviewdate.HasValue)
            entity.Nextreviewdate = dto.Nextreviewdate;

        if (dto.Lastreviewdate.HasValue)
            entity.Lastreviewdate = dto.Lastreviewdate;

        if (!string.IsNullOrEmpty(dto.Reviewfrequency))
            entity.Reviewfrequency = dto.Reviewfrequency;

        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static CareplanResponseDto ToResponseDto(Careplan entity)
    {
        // Build human-readable target labels
        string? bpLabel = entity.Bpsystolictarget.HasValue &&
                          entity.Bpdiastolictarget.HasValue
            ? $"<{entity.Bpsystolictarget}/{entity.Bpdiastolictarget} mmHg"
            : null;

        string? glucoseLabel = entity.Glucosetargetmin.HasValue &&
                               entity.Glucosetargetmax.HasValue
            ? $"{entity.Glucosetargetmin}-{entity.Glucosetargetmax} mg/dL"
            : null;

        string? hrLabel = entity.Heartratetargetmin.HasValue &&
                          entity.Heartratetargetmax.HasValue
            ? $"{entity.Heartratetargetmin}-{entity.Heartratetargetmax} bpm"
            : null;

        return new CareplanResponseDto
        {
            Careplanid = entity.Careplanid,

            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                                    ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                                      .Replace("  ", " ").Trim()
                                    : null,
            Mrn = entity.Patient?.Mrn,
            Gender = entity.Patient?.Gender,
            Dateofbirth = entity.Patient?.Dateofbirth,

            // Provider
            Providerinfoid = entity.Providerinfoid,
            Providername = entity.Providerinfo != null
                                    ? $"{entity.Providerinfo.Firstname} {entity.Providerinfo.Lastname}"
                                      .Trim()
                                    : null,
            Providerspeciality = entity.Providerinfo?.Providerprofile?.Speciality1,

            // Plan
            Startdate = entity.Startdate,
            Enddate = entity.Enddate,
            Status = entity.Status,
            Risklevel = entity.Risklevel,
            Ccmminutes = entity.Ccmminutes,
            CcmStatus = GetCcmStatus(entity.Ccmminutes),

            // BP Targets
            Bpsystolictarget = entity.Bpsystolictarget,
            Bpdiastolictarget = entity.Bpdiastolictarget,
            BptargetLabel = bpLabel,

            // Glucose Targets
            Hba1ctarget = entity.Hba1ctarget,
            Glucosetargetmin = entity.Glucosetargetmin,
            Glucosetargetmax = entity.Glucosetargetmax,
            GlucosetargetLabel = glucoseLabel,

            // Vitals Targets
            Spo2target = entity.Spo2target,
            Heartratetargetmin = entity.Heartratetargetmin,
            Heartratetargetmax = entity.Heartratetargetmax,
            HeartratetargetLabel = hrLabel,

            // Weight Targets
            Weighttarget = entity.Weighttarget,
            Bmitarget = entity.Bmitarget,

            // Lipid
            Ldltarget = entity.Ldltarget,

            // Clinical
            Problems = entity.Problems,
            Goals = entity.Goals,
            Interventions = entity.Interventions,
            Medications = entity.Medications,
            Allergies = entity.Allergies,

            // Review
            Nextreviewdate = entity.Nextreviewdate,
            Lastreviewdate = entity.Lastreviewdate,
            Reviewfrequency = entity.Reviewfrequency,
            IsOverdueForReview = GetIsOverdueForReview(entity.Nextreviewdate),
            DaysUntilNextReview = GetDaysUntilNextReview(entity.Nextreviewdate),

            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat
        };
    }
}