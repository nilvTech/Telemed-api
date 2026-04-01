// Mappers/AppointmentNoteMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class AppointmentNoteMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Appointmentnote ToEntity(CreateAppointmentNoteDto dto)
    {
        return new Appointmentnote
        {
            Appointmentid = dto.Appointmentid,
            Notes = dto.Notes,
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Appointmentnote entity,
        UpdateAppointmentNoteDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;
    }

    public static AppointmentNoteResponseDto ToResponseDto(
        Appointmentnote entity)
    {
        return new AppointmentNoteResponseDto
        {
            Id = entity.Id,
            Appointmentid = entity.Appointmentid,

            // Appointment Info
            Appointmentdate = entity.Appointment?.Appointmentdate,
            Visittype = entity.Appointment?.Visittype,
            Status = entity.Appointment?.Status,

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

            // Note Details
            Notes = entity.Notes,
            Createdat = entity.Createdat
        };
    }
}