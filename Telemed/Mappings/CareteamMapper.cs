// Mappers/CareteamMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class CareteamMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Careteam ToEntity(CreateCareteamDto dto)
    {
        return new Careteam
        {
            Teamname = dto.Teamname,
            Description = dto.Description,
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Careteam entity, UpdateCareteamDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Teamname))
            entity.Teamname = dto.Teamname;

        if (!string.IsNullOrEmpty(dto.Description))
            entity.Description = dto.Description;
    }

    public static CareteamResponseDto ToResponseDto(Careteam entity)
    {
        return new CareteamResponseDto
        {
            Careteamid = entity.Careteamid,
            Teamname = entity.Teamname,
            Description = entity.Description,
            Createdat = entity.Createdat
        };
    }
}