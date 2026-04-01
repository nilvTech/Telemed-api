// Mappers/AppointmentDocumentMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class AppointmentDocumentMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Appointmentdocument ToEntity(CreateAppointmentDocumentDto dto)
    {
        return new Appointmentdocument
        {
            Appointmentid = dto.Appointmentid,
            Fileurl = dto.Fileurl,
            Filetype = dto.Filetype,
            Uploadedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Appointmentdocument entity,
        UpdateAppointmentDocumentDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Fileurl))
            entity.Fileurl = dto.Fileurl;

        if (!string.IsNullOrEmpty(dto.Filetype))
            entity.Filetype = dto.Filetype;
    }

    public static AppointmentDocumentResponseDto ToResponseDto(
        Appointmentdocument entity)
    {
        // Extract filename from URL
        string? filename = null;
        if (!string.IsNullOrEmpty(entity.Fileurl))
        {
            filename = Path.GetFileName(entity.Fileurl);
        }

        return new AppointmentDocumentResponseDto
        {
            Id = entity.Id,
            Appointmentid = entity.Appointmentid,

            // Appointment Info
            Appointmentdate = entity.Appointment?.Appointmentdate,
            Visittype = entity.Appointment?.Visittype,
            Visitreason = entity.Appointment?.Visitreason,

            // Patient Info
            Patientid = entity.Appointment?.Patientid,
            Patientname = entity.Appointment?.Patient != null
                             ? $"{entity.Appointment.Patient.Firstname} {entity.Appointment.Patient.Middlename} {entity.Appointment.Patient.Lastname}"
                               .Replace("  ", " ").Trim()
                             : null,
            Mrn = entity.Appointment?.Patient?.Mrn,

            // Provider Info
            Providerid = entity.Appointment?.Providerid,
            Providername = entity.Appointment?.Provider?.Providername,

            // Document Details
            Fileurl = entity.Fileurl,
            Filetype = entity.Filetype,
            Filename = filename,
            Uploadedat = entity.Uploadedat
        };
    }
}