// Mappers/ConsultationDiagnosisMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ConsultationDiagnosisMapper
{
    public static Consultationdiagnosis ToEntity(
        CreateConsultationDiagnosisDto dto)
    {
        return new Consultationdiagnosis
        {
            Consultationid = dto.Consultationid,
            Diagnosiscode = dto.Diagnosiscode,
            Diagnosisname = dto.Diagnosisname
        };
    }

    public static void UpdateEntity(
        Consultationdiagnosis entity,
        UpdateConsultationDiagnosisDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Diagnosiscode))
            entity.Diagnosiscode = dto.Diagnosiscode;

        if (!string.IsNullOrEmpty(dto.Diagnosisname))
            entity.Diagnosisname = dto.Diagnosisname;
    }

    public static ConsultationDiagnosisResponseDto ToResponseDto(
        Consultationdiagnosis entity)
    {
        return new ConsultationDiagnosisResponseDto
        {
            Id = entity.Id,
            Consultationid = entity.Consultationid,

            // Consultation Info
            Consultationstatus = entity.Consultation?.Status,

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

            // Diagnosis Details
            Diagnosiscode = entity.Diagnosiscode,
            Diagnosisname = entity.Diagnosisname
        };
    }
}