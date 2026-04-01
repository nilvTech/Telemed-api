// Mappers/AppointmentMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class AppointmentMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Appointment ToEntity(CreateAppointmentDto dto)
    {
        return new Appointment
        {
            Patientid = dto.Patientid,
            Providerid = dto.Providerid,
            Clinicid = dto.Clinicid,
            Appointmentdate = dto.Appointmentdate,
            Starttime = dto.Starttime,
            Endtime = dto.Endtime,
            Visittype = dto.Visittype,
            Visitmode = dto.Visitmode,
            Visitreason = dto.Visitreason,
            Status = "Scheduled",
            Priority = dto.Priority ?? "Normal",
            Isfollowup = dto.Isfollowup,
            Parentappointmentid = dto.Parentappointmentid,
            Meetinglink = dto.Meetinglink,
            Meetingid = dto.Meetingid,
            Ispaid = false,
            Isactive = true,
            Createdby = dto.Createdby,
            Createddate = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Appointment entity, UpdateAppointmentDto dto)
    {
        if (dto.Appointmentdate.HasValue)
            entity.Appointmentdate = dto.Appointmentdate.Value;

        if (dto.Starttime.HasValue)
            entity.Starttime = dto.Starttime.Value;

        if (dto.Endtime.HasValue)
            entity.Endtime = dto.Endtime.Value;

        if (!string.IsNullOrEmpty(dto.Visittype))
            entity.Visittype = dto.Visittype;

        if (!string.IsNullOrEmpty(dto.Visitmode))
            entity.Visitmode = dto.Visitmode;

        if (!string.IsNullOrEmpty(dto.Visitreason))
            entity.Visitreason = dto.Visitreason;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (!string.IsNullOrEmpty(dto.Priority))
            entity.Priority = dto.Priority;

        if (dto.Tokennumber.HasValue)
            entity.Tokennumber = dto.Tokennumber;

        if (dto.Queueposition.HasValue)
            entity.Queueposition = dto.Queueposition;

        if (dto.Checkedintime.HasValue)
            entity.Checkedintime = ToUnspecified(dto.Checkedintime.Value);

        if (dto.Waitingminutes.HasValue)
            entity.Waitingminutes = dto.Waitingminutes;

        if (!string.IsNullOrEmpty(dto.Meetinglink))
            entity.Meetinglink = dto.Meetinglink;

        if (!string.IsNullOrEmpty(dto.Meetingid))
            entity.Meetingid = dto.Meetingid;

        if (dto.Ispaid.HasValue)
            entity.Ispaid = dto.Ispaid.Value;

        if (dto.Paymentid.HasValue)
            entity.Paymentid = dto.Paymentid;

        if (dto.Isfollowup.HasValue)
            entity.Isfollowup = dto.Isfollowup.Value;

        if (dto.Isactive.HasValue)
            entity.Isactive = dto.Isactive.Value;

        entity.Updatedby = dto.Updatedby;
        entity.Updateddate = ToUnspecified(DateTime.UtcNow);
    }

    public static AppointmentResponseDto ToResponseDto(Appointment entity)
    {
        return new AppointmentResponseDto
        {
            Appointmentid = entity.Appointmentid,

            // Patient Info
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                                  ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                                    .Replace("  ", " ").Trim()
                                  : null,
            Patientemail = entity.Patient?.Email,
            Patientphone = entity.Patient?.Phone,
            Mrn = entity.Patient?.Mrn,

            // Provider Info
            Providerid = entity.Providerid,
            Providername = entity.Provider?.Providername,
            Speciality = entity.Provider?.Speciality,
            Provideremail = entity.Provider?.Email,

            // Appointment Details
            Clinicid = entity.Clinicid,
            Appointmentdate = entity.Appointmentdate,
            Starttime = entity.Starttime,
            Endtime = entity.Endtime,
            Visittype = entity.Visittype,
            Visitmode = entity.Visitmode,
            Visitreason = entity.Visitreason,
            Status = entity.Status,
            Priority = entity.Priority,

            // Queue Info
            Tokennumber = entity.Tokennumber,
            Queueposition = entity.Queueposition,
            Checkedintime = entity.Checkedintime,
            Waitingminutes = entity.Waitingminutes,

            // Telehealth Info
            Meetinglink = entity.Meetinglink,
            Meetingid = entity.Meetingid,

            // Payment Info
            Ispaid = entity.Ispaid,
            Paymentid = entity.Paymentid,

            // Follow-up Info
            Isfollowup = entity.Isfollowup,
            Parentappointmentid = entity.Parentappointmentid,

            // Audit
            Isactive = entity.Isactive,
            Createddate = entity.Createddate,
            Updateddate = entity.Updateddate
        };
    }
}