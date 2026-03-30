using System;
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers
{
    public static class VitalMapper
    {
        private static DateTime ToUnspecified(DateTime dt)
            => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

        private static string GetBmiCategory(decimal? bmi)
        {
            if (bmi == null) return "Unknown";
            return bmi switch
            {
                < 18.5m => "Underweight",
                < 25.0m => "Normal",
                < 30.0m => "Overweight",
                _ => "Obese"
            };
        }

        // Map Create DTO → Entity
        public static Vital ToEntity(CreateVitalDto dto)
        {
            decimal? bmi = dto.Bmi;
            if (bmi == null && dto.Height.HasValue && dto.Weight.HasValue && dto.Height > 0)
            {
                decimal heightInMeters = dto.Height.Value / 100;
                bmi = Math.Round(dto.Weight.Value / (heightInMeters * heightInMeters), 2);
            }

            return new Vital
            {
                Encounterid = dto.Encounterid,
                Patientid = dto.Patientid,
                Heartrate = dto.Heartrate,
                Bloodpressure = dto.Bloodpressure,
                Respiratoryrate = dto.Respiratoryrate,
                Temperature = dto.Temperature,
                Oxygensaturation = dto.Oxygensaturation,
                Height = dto.Height,
                Weight = dto.Weight,
                Bmi = bmi,
                Recordedat = ToUnspecified(DateTime.UtcNow)
            };
        }

        // Update existing entity from DTO
        public static void UpdateEntity(Vital entity, UpdateVitalDto dto)
        {
            if (dto.Heartrate.HasValue) entity.Heartrate = dto.Heartrate;
            if (!string.IsNullOrEmpty(dto.Bloodpressure)) entity.Bloodpressure = dto.Bloodpressure;
            if (dto.Respiratoryrate.HasValue) entity.Respiratoryrate = dto.Respiratoryrate;
            if (dto.Temperature.HasValue) entity.Temperature = dto.Temperature;
            if (dto.Oxygensaturation.HasValue) entity.Oxygensaturation = dto.Oxygensaturation;
            if (dto.Height.HasValue) entity.Height = dto.Height;
            if (dto.Weight.HasValue) entity.Weight = dto.Weight;

            // Recalculate BMI if height or weight changes
            if ((dto.Height.HasValue || dto.Weight.HasValue) && entity.Height.HasValue && entity.Weight.HasValue && entity.Height > 0)
            {
                decimal heightInMeters = entity.Height.Value / 100;
                entity.Bmi = Math.Round(entity.Weight.Value / (heightInMeters * heightInMeters), 2);
            }

            if (dto.Bmi.HasValue) entity.Bmi = dto.Bmi;
        }

        // Map Entity → Response DTO
        public static VitalResponseDto ToResponseDto(Vital entity)
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

            return new VitalResponseDto
            {
                Vitalsid = entity.Vitalsid,
                Encounterid = entity.Encounterid,

                // Patient Info
                Patientid = entity.Patientid,
                Patientname = patientName,
                Dateofbirth = dob,
                Gender = entity.Patient?.Gender,

                // Vitals
                Heartrate = entity.Heartrate,
                Bloodpressure = entity.Bloodpressure,
                Respiratoryrate = entity.Respiratoryrate,
                Temperature = entity.Temperature,
                Oxygensaturation = entity.Oxygensaturation,
                Height = entity.Height,
                Weight = entity.Weight,
                Bmi = entity.Bmi,
                Bmicategory = GetBmiCategory(entity.Bmi),
                Recordedat = entity.Recordedat
            };
        }
    }
}