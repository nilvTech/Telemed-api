using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class PatientConditionMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static PatientCondition ToEntity(CreatePatientConditionDto dto)
    {
        return new PatientCondition
        {
            PatientId = dto.PatientId,
            ConditionId = dto.ConditionId,
            ConsultationId = dto.ConsultationId,
            ProviderInfoId = dto.ProviderInfoId,

            Status = dto.Status ?? "Active",
            OnsetDate = dto.OnsetDate.HasValue
                ? ToUnspecified(dto.OnsetDate.Value)
                : null,
            Note = dto.Note,
            ManagedBy = dto.ManagedBy,
            CreatedBy = dto.CreatedBy,
            CreatedAt = ToUnspecified(DateTime.UtcNow),
            UpdatedAt = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        PatientCondition entity,
        UpdatePatientConditionDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (dto.OnsetDate.HasValue)
            entity.OnsetDate = ToUnspecified(dto.OnsetDate.Value);

        if (!string.IsNullOrEmpty(dto.Note))
            entity.Note = dto.Note;

        if (!string.IsNullOrEmpty(dto.ManagedBy))
            entity.ManagedBy = dto.ManagedBy;

        entity.UpdatedBy = dto.UpdatedBy;
        entity.UpdatedAt = ToUnspecified(DateTime.UtcNow);
    }

    public static PatientConditionResponseDto ToResponseDto(PatientCondition entity)
    {
        return new PatientConditionResponseDto
        {
            PatientConditionId = entity.PatientConditionId,

            // Patient
            PatientId = entity.PatientId,
            PatientName = entity.Patient != null
                ? $"{entity.Patient.Firstname} {entity.Patient.Lastname}".Trim()
                : null,
            Mrn = entity.Patient?.Mrn,

            // Condition
            ConditionId = entity.ConditionId,
            ConditionName = entity.ConditionMaster?.ConditionName,
            IcdCode = entity.ConditionMaster?.IcdCode,
            Description = entity.ConditionMaster?.Description,
            Type = entity.ConditionMaster?.Type,

            // Provider
            ProviderInfoId = entity.ProviderInfoId,
            ProviderName = entity.ProviderInfo != null
                ? $"{entity.ProviderInfo.Firstname} {entity.ProviderInfo.Lastname}"
                : null,

            // ✅ FIXED CONSULTATION
           // ConsultationId = entity.ConsultationId,
            //ConsultationDate = entity.Consultation != null
            //    ? entity.Consultation.Createddate   // ✅ correct property
             //   : null,

            // Details
            Status = entity.Status,
            OnsetDate = entity.OnsetDate,
            Note = entity.Note,
            ManagedBy = entity.ManagedBy,

            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}