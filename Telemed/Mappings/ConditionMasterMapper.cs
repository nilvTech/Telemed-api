// Mappers/ConditionMasterMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ConditionMasterMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static ConditionMaster ToEntity(CreateConditionMasterDto dto)
    {
        return new ConditionMaster
        {
            ConditionName = dto.ConditionName,
            IcdCode = dto.IcdCode,
            Description = dto.Description,
            Type = dto.Type,
            CreatedAt = ToUnspecified(DateTime.UtcNow),
            UpdatedAt = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        ConditionMaster entity,
        UpdateConditionMasterDto dto)
    {
        if (!string.IsNullOrEmpty(dto.ConditionName))
            entity.ConditionName = dto.ConditionName;

        if (!string.IsNullOrEmpty(dto.Description))
            entity.Description = dto.Description;

        if (!string.IsNullOrEmpty(dto.Type))
            entity.Type = dto.Type;

        entity.UpdatedAt = ToUnspecified(DateTime.UtcNow);
    }

    public static ConditionMasterResponseDto ToResponseDto(ConditionMaster entity)
    {
        return new ConditionMasterResponseDto
        {
            ConditionId = entity.ConditionId,
            ConditionName = entity.ConditionName,
            IcdCode = entity.IcdCode,
            Description = entity.Description,
            Type = entity.Type,
            TotalPatients = entity.PatientConditions.Count,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}