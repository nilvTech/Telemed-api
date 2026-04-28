// Mappers/AdminclaimMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class AdminclaimMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Adminclaim ToEntity(CreateAdminclaimDto dto)
    {
        return new Adminclaim
        {
            Claimid = dto.Claimid,
            Patientid = dto.Patientid,
            Providerinfoid = dto.Providerinfoid,
            Appointmentid = dto.Appointmentid,
            Encounterid = dto.Encounterid,
            Claimdate = dto.Claimdate.HasValue
                             ? ToUnspecified(dto.Claimdate.Value)
                             : ToUnspecified(DateTime.UtcNow),
            Status = dto.Status ?? "Submitted",
            Lastaction = dto.Lastaction,
            Lastactiondate = !string.IsNullOrEmpty(dto.Lastaction)
                             ? ToUnspecified(DateTime.UtcNow)
                             : null,
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Adminclaim entity, UpdateAdminclaimDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (!string.IsNullOrEmpty(dto.Lastaction))
        {
            entity.Lastaction = dto.Lastaction;
            entity.Lastactiondate = ToUnspecified(DateTime.UtcNow);
        }

        if (dto.Lastactiondate.HasValue)
            entity.Lastactiondate = ToUnspecified(dto.Lastactiondate.Value);

        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static AdminclaimResponseDto ToResponseDto(Adminclaim entity)
    {
        return new AdminclaimResponseDto
        {
            Adminclaimid = entity.Adminclaimid,

            // Claim
            Claimid = entity.Claimid,
            Claimnumber = entity.Claim?.Claimnumber,
            Claimtotalamount = entity.Claim?.Totalamount,
            Claimpayer = entity.Claim?.Payer,
            Claimstatus = entity.Claim?.Status,

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
            Providerspeciality = entity.Providerinfo?.Providerprofile?.Speciality1,

            // Appointment
            Appointmentid = entity.Appointmentid,
            Appointmentdate = entity.Appointment?.Appointmentdate,

            // Encounter
            Encounterid = entity.Encounterid,
            Encounterdate = entity.Encounter?.Encounterdate,

            // Admin Claim
            Claimdate = entity.Claimdate,
            Status = entity.Status,
            Lastaction = entity.Lastaction,
            Lastactiondate = entity.Lastactiondate,
            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat
        };
    }
}