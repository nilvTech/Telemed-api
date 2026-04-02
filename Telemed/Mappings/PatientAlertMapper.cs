// Mappers/PatientAlertMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class PatientAlertMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Patientalert ToEntity(CreatePatientAlertDto dto)
    {
        return new Patientalert
        {
            Patientid = dto.Patientid,
            Alerttype = dto.Alerttype,
            Alertmessage = dto.Alertmessage,
            Severity = dto.Severity ?? "Low",
            Isread = false,
            Isacknowledged = false,
            Isactive = true,
            Createddate = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Patientalert entity,
        UpdatePatientAlertDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Alerttype))
            entity.Alerttype = dto.Alerttype;

        if (!string.IsNullOrEmpty(dto.Alertmessage))
            entity.Alertmessage = dto.Alertmessage;

        if (!string.IsNullOrEmpty(dto.Severity))
            entity.Severity = dto.Severity;

        if (dto.Isread.HasValue)
            entity.Isread = dto.Isread.Value;

        if (dto.Isacknowledged.HasValue)
            entity.Isacknowledged = dto.Isacknowledged;

        if (dto.Isactive.HasValue)
            entity.Isactive = dto.Isactive.Value;

        entity.Updateddate = ToUnspecified(DateTime.UtcNow);
    }

    public static PatientAlertResponseDto ToResponseDto(Patientalert entity)
    {
        return new PatientAlertResponseDto
        {
            Alertid = entity.Alertid,

            // Patient Info
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                             ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                               .Replace("  ", " ").Trim()
                             : null,
            Patientemail = entity.Patient?.Email,
            Mrn = entity.Patient?.Mrn,

            // Alert Details
            Alerttype = entity.Alerttype,
            Alertmessage = entity.Alertmessage,
            Severity = entity.Severity,
            Isread = entity.Isread,
            Isacknowledged = entity.Isacknowledged,
            Isactive = entity.Isactive,
            Createddate = entity.Createddate,
            Updateddate = entity.Updateddate
        };
    }
}