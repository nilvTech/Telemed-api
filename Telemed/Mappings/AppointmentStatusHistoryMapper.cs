// Mappers/AppointmentStatusHistoryMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class AppointmentStatusHistoryMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Appointmentstatushistory ToEntity(
        CreateAppointmentStatusHistoryDto dto)
    {
        return new Appointmentstatushistory
        {
            Appointmentid = dto.Appointmentid,
            Status = dto.Status,
            Changedat = ToUnspecified(DateTime.UtcNow),
            Changedby = dto.Changedby
        };
    }

    public static AppointmentStatusHistoryResponseDto ToResponseDto(
        Appointmentstatushistory entity)
    {
        return new AppointmentStatusHistoryResponseDto
        {
            Id = entity.Id,
            Appointmentid = entity.Appointmentid,

            // Appointment Info
            Appointmentdate = entity.Appointment?.Appointmentdate,
            Visittype = entity.Appointment?.Visittype,

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

            // Status History
            Status = entity.Status,
            Changedat = entity.Changedat,
            Changedby = entity.Changedby
        };
    }
}