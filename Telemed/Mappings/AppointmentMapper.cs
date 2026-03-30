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
            Scheduleddatetime = ToUnspecified(dto.Scheduleddatetime),
            Mode = dto.Mode ?? "Telemedicine",
            Notes = dto.Notes,
            Status = "Pending",
            Createdate = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Appointment entity, UpdateAppointmentDto dto)
    {
        if (dto.Scheduleddatetime.HasValue)
            entity.Scheduleddatetime = ToUnspecified(dto.Scheduleddatetime.Value);

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (!string.IsNullOrEmpty(dto.Mode))
            entity.Mode = dto.Mode;

        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;

        entity.Updatedate = ToUnspecified(DateTime.UtcNow);
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

            // Provider Info
            Providerid = entity.Providerid,
            Providername = entity.Provider?.Providername,
            Speciality = entity.Provider?.Speciality,

            Scheduleddatetime = entity.Scheduleddatetime,
            Status = entity.Status,
            Mode = entity.Mode,
            Notes = entity.Notes,
            Createdat = entity.Createdate,   
            Updatedat = entity.Updatedate    
        };
    }
}