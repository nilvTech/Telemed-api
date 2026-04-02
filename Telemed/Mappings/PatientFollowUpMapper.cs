// Mappers/PatientFollowUpMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class PatientFollowUpMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Patientfollowup ToEntity(CreatePatientFollowUpDto dto)
    {
        return new Patientfollowup
        {
            Patientid = dto.Patientid,
            Followupdate = ToUnspecified(dto.Followupdate),
            Notes = dto.Notes,
            Status = "Pending",
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Patientfollowup entity,
        UpdatePatientFollowUpDto dto)
    {
        if (dto.Followupdate.HasValue)
            entity.Followupdate = ToUnspecified(dto.Followupdate.Value);

        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;
    }

    public static PatientFollowUpResponseDto ToResponseDto(
        Patientfollowup entity)
    {
        return new PatientFollowUpResponseDto
        {
            Id = entity.Id,

            // Patient Info
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                           ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                             .Replace("  ", " ").Trim()
                           : null,
            Patientemail = entity.Patient?.Email,
            Mrn = entity.Patient?.Mrn,

            // FollowUp Details
            Followupdate = entity.Followupdate,
            Notes = entity.Notes,
            Status = entity.Status,
            Createdat = entity.Createdat
        };
    }
}