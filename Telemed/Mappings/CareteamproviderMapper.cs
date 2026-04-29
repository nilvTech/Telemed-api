// Mappers/CareteamproviderMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class CareteamproviderMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Careteamprovider ToEntity(CreateCareteamproviderDto dto)
    {
        return new Careteamprovider
        {
            Careteamid = dto.Careteamid,
            Providerinfoid = dto.Providerinfoid,
            Role = dto.Role,
            Assigneddate = ToUnspecified(DateTime.UtcNow),
            Isactive = true
        };
    }

    public static void UpdateEntity(
        Careteamprovider entity,
        UpdateCareteamproviderDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Role))
            entity.Role = dto.Role;

        if (dto.Isactive.HasValue)
            entity.Isactive = dto.Isactive;
    }

    public static CareteamproviderResponseDto ToResponseDto(
        Careteamprovider entity)
    {
        return new CareteamproviderResponseDto
        {
            Careteamproviderid = entity.Careteamproviderid,

            // Careteam
            Careteamid = entity.Careteamid,
            Teamname = entity.Careteam?.Teamname,
            Teamdescription = entity.Careteam?.Description,

            // Provider
            Providerinfoid = entity.Providerinfoid,
            Providername = entity.Providerinfo != null
                                 ? $"{entity.Providerinfo.Firstname} {entity.Providerinfo.Lastname}"
                                   .Trim()
                                 : null,
            Provideremail = entity.Providerinfo?.Email,
            Providerphone = entity.Providerinfo?.Phone,
            Speciality1 = entity.Providerinfo?.Providerprofile?.Speciality1,
            Speciality2 = entity.Providerinfo?.Providerprofile?.Speciality2,
            Providertype = entity.Providerinfo?.Providerprofile?.Providertype,
            NpiNumber = entity.Providerinfo?.Providerprofile?.NpiNumber,

            // Assignment
            Role = entity.Role,
            Assigneddate = entity.Assigneddate,
            Isactive = entity.Isactive,
            StatusLabel = entity.Isactive == true ? "Active" : "Inactive"
        };
    }
}