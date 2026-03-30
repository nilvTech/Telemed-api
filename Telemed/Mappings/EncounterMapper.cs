// Mappers/EncounterMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class EncounterMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Encounter ToEntity(CreateEncounterDto dto)
    {
        return new Encounter
        {
            Appointmentid = dto.Appointmentid,
            Patientid = dto.Patientid,
            Providerid = dto.Providerid,
            Encounterdate = ToUnspecified(dto.Encounterdate),
            Subjective = dto.Subjective,
            Objective = dto.Objective,
            Assessment = dto.Assessment,
            Plan = dto.Plan,
            Diagnosis = dto.Diagnosis,
            Icd10code = dto.Icd10code,
            Notes = dto.Notes,
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Encounter entity, UpdateEncounterDto dto)
    {
        if (dto.Encounterdate.HasValue)
            entity.Encounterdate = ToUnspecified(dto.Encounterdate.Value);

        if (!string.IsNullOrEmpty(dto.Subjective))
            entity.Subjective = dto.Subjective;

        if (!string.IsNullOrEmpty(dto.Objective))
            entity.Objective = dto.Objective;

        if (!string.IsNullOrEmpty(dto.Assessment))
            entity.Assessment = dto.Assessment;

        if (!string.IsNullOrEmpty(dto.Plan))
            entity.Plan = dto.Plan;

        if (!string.IsNullOrEmpty(dto.Diagnosis))
            entity.Diagnosis = dto.Diagnosis;

        if (!string.IsNullOrEmpty(dto.Icd10code))
            entity.Icd10code = dto.Icd10code;

        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;

        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static EncounterResponseDto ToResponseDto(Encounter entity)
    {
        return new EncounterResponseDto
        {
            Encounterid = entity.Encounterid,

            // Appointment
            Appointmentid = entity.Appointmentid,
            Appointmentdate = entity.Appointment?.Scheduleddatetime,

            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                              ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}".Replace("  ", " ").Trim()
                              : null,

            // Provider
            Providerid = entity.Providerid,
            Providername = entity.Provider?.Providername,
            Speciality = entity.Provider?.Speciality,

            Encounterdate = entity.Encounterdate,
            Subjective = entity.Subjective,
            Objective = entity.Objective,
            Assessment = entity.Assessment,
            Plan = entity.Plan,
            Diagnosis = entity.Diagnosis,
            Icd10code = entity.Icd10code,
            Notes = entity.Notes,
            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat
        };
    }
}