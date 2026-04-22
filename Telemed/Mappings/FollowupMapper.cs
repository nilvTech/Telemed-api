// Mappers/FollowupMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class FollowupMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    private static string GetDuedateStatus(DateOnly followupdate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (followupdate < today) return "Overdue";
        if (followupdate == today) return "Today";
        return "Upcoming";
    }

    private static int GetDaysUntilFollowup(DateOnly followupdate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return followupdate.DayNumber - today.DayNumber;
    }

    public static Followup ToEntity(CreateFollowupDto dto)
    {
        return new Followup
        {
            Patientid = dto.Patientid,
            Appointmentid = dto.Appointmentid,
            Followupdate = dto.Followupdate,
            Followuptype = dto.Followuptype,
            Notes = dto.Notes,
            Status = dto.Status ?? "Scheduled",
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Followup entity, UpdateFollowupDto dto)
    {
        if (dto.Followupdate.HasValue)
            entity.Followupdate = dto.Followupdate.Value;

        if (!string.IsNullOrEmpty(dto.Followuptype))
            entity.Followuptype = dto.Followuptype;

        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;
    }

    public static FollowupResponseDto ToResponseDto(Followup entity)
    {
        return new FollowupResponseDto
        {
            Followupid = entity.Followupid,

            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                                  ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                                    .Replace("  ", " ").Trim()
                                  : null,
            Mrn = entity.Patient?.Mrn,

            // Appointment
            Appointmentid = entity.Appointmentid,
            Appointmentdate = entity.Appointment?.Appointmentdate,
            Appointmentstatus = entity.Appointment?.Status,

            // Followup
            Followupdate = entity.Followupdate,
            DuedateStatus = GetDuedateStatus(entity.Followupdate),
            DaysUntilFollowup = GetDaysUntilFollowup(entity.Followupdate),
            Followuptype = entity.Followuptype,
            Notes = entity.Notes,
            Status = entity.Status,
            Createdat = entity.Createdat
        };
    }
}