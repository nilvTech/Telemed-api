using System;
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers
{
    public static class PrescriptionMapper
    {
        private static DateTime ToUnspecified(DateTime dt)
            => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

        // Map Create DTO → Entity
        public static Prescription ToEntity(CreatePrescriptionDto dto)
        {
            return new Prescription
            {
                Encounterid = dto.Encounterid,
                Patientid = dto.Patientid,
                Providerid = dto.Providerid,
                Medicinename = dto.Medicinename,
                Dosage = dto.Dosage,
                Frequency = dto.Frequency,
                Duration = dto.Duration,
                Notes = dto.Notes,
                Createdat = ToUnspecified(DateTime.UtcNow)
            };
        }

        // Update existing entity from DTO
        public static void UpdateEntity(Prescription entity, UpdatePrescriptionDto dto)
        {
            if (!string.IsNullOrEmpty(dto.Medicinename))
                entity.Medicinename = dto.Medicinename;

            if (!string.IsNullOrEmpty(dto.Dosage))
                entity.Dosage = dto.Dosage;

            if (!string.IsNullOrEmpty(dto.Frequency))
                entity.Frequency = dto.Frequency;

            if (!string.IsNullOrEmpty(dto.Duration))
                entity.Duration = dto.Duration;

            if (!string.IsNullOrEmpty(dto.Notes))
                entity.Notes = dto.Notes;
        }

        // Map Entity → Response DTO
        public static PrescriptionResponseDto ToResponseDto(Prescription entity)
        {
            string patientName = null;
            if (entity.Patient != null)
            {
                patientName = $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                              .Replace("  ", " ").Trim();
            }

            DateTime? dob = null;
            if (entity.Patient?.Dateofbirth != null)
            {
                // Convert DateOnly? → DateTime?
                dob = entity.Patient.Dateofbirth.Value.ToDateTime(TimeOnly.MinValue);
            }

            return new PrescriptionResponseDto
            {
                Prescriptionid = entity.Prescriptionid,

                // Encounter Info
                Encounterid = entity.Encounterid,
                Diagnosis = entity.Encounter?.Diagnosis,
                Icd10code = entity.Encounter?.Icd10code,

                // Patient Info
                Patientid = entity.Patientid,
                Patientname = patientName,
                Dateofbirth = dob, // Converted safely
                Gender = entity.Patient?.Gender,

                // Provider Info
                Providerid = entity.Providerid,
                Providername = entity.Provider?.Providername,
                Speciality = entity.Provider?.Speciality,

                // Prescription Details
                Medicinename = entity.Medicinename,
                Dosage = entity.Dosage,
                Frequency = entity.Frequency,
                Duration = entity.Duration,
                Notes = entity.Notes,
                Createdat = entity.Createdat
            };
        }
    }
}