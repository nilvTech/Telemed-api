// Mappers/ConsultationPrescriptionMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ConsultationPrescriptionMapper
{
    public static Consultationprescription ToEntity(
        CreateConsultationPrescriptionDto dto)
    {
        return new Consultationprescription
        {
            Consultationid = dto.Consultationid,
            Medicationname = dto.Medicationname,
            Dosage = dto.Dosage,
            Frequency = dto.Frequency,
            Duration = dto.Duration,
            Instructions = dto.Instructions
        };
    }

    public static void UpdateEntity(
        Consultationprescription entity,
        UpdateConsultationPrescriptionDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Medicationname))
            entity.Medicationname = dto.Medicationname;

        if (!string.IsNullOrEmpty(dto.Dosage))
            entity.Dosage = dto.Dosage;

        if (!string.IsNullOrEmpty(dto.Frequency))
            entity.Frequency = dto.Frequency;

        if (!string.IsNullOrEmpty(dto.Duration))
            entity.Duration = dto.Duration;

        if (!string.IsNullOrEmpty(dto.Instructions))
            entity.Instructions = dto.Instructions;
    }

    public static ConsultationPrescriptionResponseDto ToResponseDto(
        Consultationprescription entity)
    {
        return new ConsultationPrescriptionResponseDto
        {
            Prescriptionid = entity.Prescriptionid,
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

            // Prescription Details
            Medicationname = entity.Medicationname,
            Dosage = entity.Dosage,
            Frequency = entity.Frequency,
            Duration = entity.Duration,
            Instructions = entity.Instructions
        };
    }
}