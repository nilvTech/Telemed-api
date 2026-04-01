// Mappers/ConsultationNoteMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ConsultationNoteMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Consultationnote ToEntity(CreateConsultationNoteDto dto)
    {
        return new Consultationnote
        {
            Consultationid = dto.Consultationid,
            Notes = dto.Notes,
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Consultationnote entity,
        UpdateConsultationNoteDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Notes))
            entity.Notes = dto.Notes;
    }

    public static ConsultationNoteResponseDto ToResponseDto(
        Consultationnote entity)
    {
        return new ConsultationNoteResponseDto
        {
            Id = entity.Id,
            Consultationid = entity.Consultationid,

            // Consultation Info
            Consultationstatus = entity.Consultation?.Status,
            Consultationstarttime = entity.Consultation?.Starttime,

            // Patient Info
            Patientid = entity.Consultation?.Patientid,
            Patientname = entity.Consultation?.Patient != null
                                      ? $"{entity.Consultation.Patient.Firstname} {entity.Consultation.Patient.Middlename} {entity.Consultation.Patient.Lastname}"
                                        .Replace("  ", " ").Trim()
                                      : null,
            Mrn = entity.Consultation?.Patient?.Mrn,

            // Provider Info
            Providerid = entity.Consultation?.Providerid,
            Providername = entity.Consultation?.Provider?.Providername,
            Speciality = entity.Consultation?.Provider?.Speciality,

            // Note Details
            Notes = entity.Notes,
            Createdat = entity.Createdat
        };
    }
}