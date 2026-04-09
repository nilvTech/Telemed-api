using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ProviderGroupMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    // ----------------------------
    // ProviderGroup Entity Mapping
    // ----------------------------
    public static ProviderGroup ToEntity(CreateProviderGroupDto dto)
    {
        return new ProviderGroup
        {
            GroupName = dto.GroupName,
            Email = dto.Email,
            Phone = dto.Phone,
            Speciality = dto.Speciality,
            Website = dto.Website,
            Bio = dto.Bio,
            AddressLine1 = dto.AddressLine1,
            AddressLine2 = dto.AddressLine2,
            City = dto.City,
            State = dto.State,
            Zip = dto.Zip,
            Country = dto.Country ?? "United States",
            IsActive = true,
            CreatedBy = dto.CreatedBy,   // keep in Group table
            CreatedAt = ToUnspecified(DateTime.UtcNow),
            UpdatedAt = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(ProviderGroup entity, UpdateProviderGroupDto dto)
    {
        if (!string.IsNullOrEmpty(dto.GroupName))
            entity.GroupName = dto.GroupName;

        if (!string.IsNullOrEmpty(dto.Phone))
            entity.Phone = dto.Phone;

        if (!string.IsNullOrEmpty(dto.Speciality))
            entity.Speciality = dto.Speciality;

        if (!string.IsNullOrEmpty(dto.Website))
            entity.Website = dto.Website;

        if (!string.IsNullOrEmpty(dto.Bio))
            entity.Bio = dto.Bio;

        if (!string.IsNullOrEmpty(dto.AddressLine1))
            entity.AddressLine1 = dto.AddressLine1;

        if (!string.IsNullOrEmpty(dto.AddressLine2))
            entity.AddressLine2 = dto.AddressLine2;

        if (!string.IsNullOrEmpty(dto.City))
            entity.City = dto.City;

        if (!string.IsNullOrEmpty(dto.State))
            entity.State = dto.State;

        if (!string.IsNullOrEmpty(dto.Zip))
            entity.Zip = dto.Zip;

        if (!string.IsNullOrEmpty(dto.Country))
            entity.Country = dto.Country;

        if (dto.IsActive.HasValue)
            entity.IsActive = dto.IsActive;

        entity.UpdatedBy = dto.UpdatedBy; // keep for Group table
        entity.UpdatedAt = ToUnspecified(DateTime.UtcNow);
    }

    public static ProviderGroupResponseDto ToResponseDto(ProviderGroup entity)
    {
        var addressParts = new List<string>();

        if (!string.IsNullOrEmpty(entity.AddressLine1))
            addressParts.Add(entity.AddressLine1);
        if (!string.IsNullOrEmpty(entity.AddressLine2))
            addressParts.Add(entity.AddressLine2);
        if (!string.IsNullOrEmpty(entity.City))
            addressParts.Add(entity.City);
        if (!string.IsNullOrEmpty(entity.State))
            addressParts.Add(entity.State);
        if (!string.IsNullOrEmpty(entity.Zip))
            addressParts.Add(entity.Zip);
        if (!string.IsNullOrEmpty(entity.Country))
            addressParts.Add(entity.Country);

        return new ProviderGroupResponseDto
        {
            GroupId = entity.GroupId,
            GroupName = entity.GroupName,
            Email = entity.Email,
            Phone = entity.Phone,
            Speciality = entity.Speciality,
            Website = entity.Website,
            Bio = entity.Bio,
            AddressLine1 = entity.AddressLine1,
            AddressLine2 = entity.AddressLine2,
            City = entity.City,
            State = entity.State,
            Zip = entity.Zip,
            Country = entity.Country,
            FullAddress = string.Join(", ", addressParts),
            IsActive = entity.IsActive,
            TotalMembers = entity.GroupMembers.Count(m => m.IsActive == true),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Members = entity.GroupMembers.Select(ToMemberSummaryDto).ToList()
        };
    }

    // ----------------------------
    // ProviderGroup Member Mapping
    // ----------------------------
    public static ProviderGroupMemberSummaryDto ToMemberSummaryDto(ProviderGroup_Member member)
    {
        return new ProviderGroupMemberSummaryDto
        {
            GroupMemberId = member.GroupMemberId,
            ProviderInfoId = member.ProviderInfoId,
            ProviderFullname = member.ProviderInfo != null
                ? $"{member.ProviderInfo.Firstname} {member.ProviderInfo.Lastname}".Trim()
                : null,
            ProviderEmail = member.ProviderInfo?.Email,
            ProviderPhone = member.ProviderInfo?.Phone,
            Speciality1 = member.ProviderInfo?.Providerprofile?.Speciality1,
            Speciality2 = member.ProviderInfo?.Providerprofile?.Speciality2,
            Providertype = member.ProviderInfo?.Providerprofile?.Providertype,
            NpiNumber = member.ProviderInfo?.Providerprofile?.NpiNumber,
            RoleInGroup = member.RoleInGroup,
            JoinDate = member.JoinDate,
            IsActive = member.IsActive
        };
    }

    public static ProviderGroup_Member ToMemberEntity(AddProviderGroupMemberDto dto)
    {
        return new ProviderGroup_Member
        {
            GroupId = dto.GroupId,
            ProviderInfoId = dto.ProviderInfoId,
            RoleInGroup = dto.RoleInGroup ?? "Member",
            JoinDate = dto.JoinDate.HasValue
                ? DateTime.SpecifyKind(dto.JoinDate.Value, DateTimeKind.Unspecified)
                : DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified),
            IsActive = true
            // REMOVED CreatedBy, CreatedAt, UpdatedBy, UpdatedAt
        };
    }

    public static void UpdateMemberEntity(ProviderGroup_Member entity, UpdateProviderGroupMemberDto dto)
    {
        if (!string.IsNullOrEmpty(dto.RoleInGroup))
            entity.RoleInGroup = dto.RoleInGroup;

        if (dto.IsActive.HasValue)
            entity.IsActive = dto.IsActive;

        // REMOVED UpdatedBy & UpdatedAt
    }

    public static ProviderGroupMemberResponseDto ToMemberResponseDto(ProviderGroup_Member entity)
    {
        return new ProviderGroupMemberResponseDto
        {
            GroupMemberId = entity.GroupMemberId,

            // Group Info
            GroupId = entity.GroupId,
            GroupName = entity.Group?.GroupName,
            GroupEmail = entity.Group?.Email,
            GroupSpeciality = entity.Group?.Speciality,
            GroupState = entity.Group?.State,

            // Provider Info
            ProviderInfoId = entity.ProviderInfoId,
            ProviderFullname = entity.ProviderInfo != null
                ? $"{entity.ProviderInfo.Firstname} {entity.ProviderInfo.Lastname}".Trim()
                : null,
            ProviderEmail = entity.ProviderInfo?.Email,
            ProviderPhone = entity.ProviderInfo?.Phone,
            Speciality1 = entity.ProviderInfo?.Providerprofile?.Speciality1,
            Speciality2 = entity.ProviderInfo?.Providerprofile?.Speciality2,
            Providertype = entity.ProviderInfo?.Providerprofile?.Providertype,
            NpiNumber = entity.ProviderInfo?.Providerprofile?.NpiNumber,

            // Membership
            RoleInGroup = entity.RoleInGroup,
            JoinDate = entity.JoinDate,
            IsActive = entity.IsActive
            // REMOVED CreatedAt / UpdatedAt
        };
    }
}