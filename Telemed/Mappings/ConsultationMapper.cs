// Mappers/ConsultationMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ConsultationMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Consultation ToEntity(CreateConsultationDto dto)
    {
        // Auto calculate duration if both times provided
        int? duration = null;
        if (dto.Starttime.HasValue && dto.Endtime.HasValue)
            duration = (int)(dto.Endtime.Value - dto.Starttime.Value).TotalMinutes;

        return new Consultation
        {
            Appointmentid = dto.Appointmentid,
            Patientid = dto.Patientid,
            Providerid = dto.Providerid,
            Starttime = dto.Starttime.HasValue
                                      ? ToUnspecified(dto.Starttime.Value)
                                      : null,
            Endtime = dto.Endtime.HasValue
                                      ? ToUnspecified(dto.Endtime.Value)
                                      : null,
            Durationminutes = duration,
            Chiefcomplaint = dto.Chiefcomplaint,
            Diagnosis = dto.Diagnosis,
            Treatmentplan = dto.Treatmentplan,
            Notes = dto.Notes,
            Temperature = dto.Temperature,
            Bloodpressure = dto.Bloodpressure,
            Pulse = dto.Pulse,
            Respiratoryrate = dto.Respiratoryrate,
            Oxygensaturation = dto.Oxygensaturation,
            Status = "InProgress",
            Followupdate = dto.Followupdate,
            Followupnotes = dto.Followupnotes,
            Recordingurl = dto.Recordingurl,
            Isprescriptiongenerated = false,
            Isactive = true,
            Createdby = dto.Createdby,
            Createddate = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Consultation entity, UpdateConsultationDto dto)
    {
        if (dto.Starttime.HasValue)
            entity.Starttime = ToUnspecified(dto.Starttime.Value);

        if (dto.Endtime.HasValue)
            entity.Endtime = ToUnspecified(dto.Endtime.Value);

        // Auto recalculate duration
        if (entity.Starttime.HasValue && entity.Endtime.HasValue)
            entity.Durationminutes = (int)(entity.Endtime.Value
                - entity.Starttime.Value).TotalMinutes;

        if (dto.Durationminutes.HasValue)
            entity.Durationminutes = dto.Durationminutes;

        if (!string.IsNullOrEmpty(dto.Chiefcomplaint))
            entity.Chiefcomplaint = dto.Chiefcomplaint;

        if (!string.IsNullOrEmpty(dto.Diagnosis))
            entity.Diagnosis = dto.Diagnosis;

        if (!string.IsNullOrEmpty(dto.Treatmentplan))
            entity.Treatmentplan = dto.Treatmentplan;

        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;

        if (dto.Temperature.HasValue)
            entity.Temperature = dto.Temperature;

        if (!string.IsNullOrEmpty(dto.Bloodpressure))
            entity.Bloodpressure = dto.Bloodpressure;

        if (dto.Pulse.HasValue)
            entity.Pulse = dto.Pulse;

        if (dto.Respiratoryrate.HasValue)
            entity.Respiratoryrate = dto.Respiratoryrate;

        if (dto.Oxygensaturation.HasValue)
            entity.Oxygensaturation = dto.Oxygensaturation;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (dto.Followupdate.HasValue)
            entity.Followupdate = dto.Followupdate;

        if (!string.IsNullOrEmpty(dto.Followupnotes))
            entity.Followupnotes = dto.Followupnotes;

        if (!string.IsNullOrEmpty(dto.Recordingurl))
            entity.Recordingurl = dto.Recordingurl;

        if (dto.Isprescriptiongenerated.HasValue)
            entity.Isprescriptiongenerated = dto.Isprescriptiongenerated.Value;

        if (dto.Isactive.HasValue)
            entity.Isactive = dto.Isactive.Value;

        entity.Updatedby = dto.Updatedby;
        entity.Updateddate = ToUnspecified(DateTime.UtcNow);
    }

    public static ConsultationResponseDto ToResponseDto(Consultation entity)
    {
        return new ConsultationResponseDto
        {
            Consultationid = entity.Consultationid,

            // Appointment Info
            Appointmentid = entity.Appointmentid,
            Appointmentdate = entity.Appointment?.Appointmentdate,
            Visittype = entity.Appointment?.Visittype,
            Visitmode = entity.Appointment?.Visitmode,

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

            // Consultation Details
            Starttime = entity.Starttime,
            Endtime = entity.Endtime,
            Durationminutes = entity.Durationminutes,
            Chiefcomplaint = entity.Chiefcomplaint,
            Diagnosis = entity.Diagnosis,
            Treatmentplan = entity.Treatmentplan,
            Notes = entity.Notes,

            // Vitals
            Temperature = entity.Temperature,
            Bloodpressure = entity.Bloodpressure,
            Pulse = entity.Pulse,
            Respiratoryrate = entity.Respiratoryrate,
            Oxygensaturation = entity.Oxygensaturation,

            Status = entity.Status,

            // Follow-up
            Followupdate = entity.Followupdate,
            Followupnotes = entity.Followupnotes,

            Recordingurl = entity.Recordingurl,
            Isprescriptiongenerated = entity.Isprescriptiongenerated,
            Isactive = entity.Isactive,
            Createddate = entity.Createddate,
            Updateddate = entity.Updateddate
        };
    }
}