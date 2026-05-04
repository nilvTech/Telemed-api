// Mappers/RoleMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class RoleMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Role ToEntity(CreateRoleDto dto)
    {
        return new Role
        {
            Rolename = dto.Rolename,
            Rolecode = dto.Rolecode.ToUpper().Trim(),
            Roletype = dto.Roletype,
            Accesslevel = dto.Accesslevel,
            Status = dto.Status ?? "Active",
            Defaultlandingpage = dto.Defaultlandingpage,
            Datascope = dto.Datascope,
            Requiresmfa = dto.Requiresmfa ?? false,
            Description = dto.Description,
            Createdby = dto.Createdby,
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Role entity, UpdateRoleDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Rolename))
            entity.Rolename = dto.Rolename;

        if (!string.IsNullOrEmpty(dto.Roletype))
            entity.Roletype = dto.Roletype;

        if (!string.IsNullOrEmpty(dto.Accesslevel))
            entity.Accesslevel = dto.Accesslevel;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (!string.IsNullOrEmpty(dto.Defaultlandingpage))
            entity.Defaultlandingpage = dto.Defaultlandingpage;

        if (!string.IsNullOrEmpty(dto.Datascope))
            entity.Datascope = dto.Datascope;

        if (dto.Requiresmfa.HasValue)
            entity.Requiresmfa = dto.Requiresmfa;

        if (!string.IsNullOrEmpty(dto.Description))
            entity.Description = dto.Description;

        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static RoleResponseDto ToResponseDto(Role entity)
    {
        return new RoleResponseDto
        {
            Roleid = entity.Roleid,
            Rolename = entity.Rolename,
            Rolecode = entity.Rolecode,
            Roletype = entity.Roletype,
            Accesslevel = entity.Accesslevel,
            Status = entity.Status,
            Defaultlandingpage = entity.Defaultlandingpage,
            Datascope = entity.Datascope,
            Requiresmfa = entity.Requiresmfa,
            Description = entity.Description,
            Createdby = entity.Createdby,
            Createdat = entity.Createdat,
            Updatedby = entity.Updatedby,
            Updatedat = entity.Updatedat
        };
    }

    public static RolesDashboardResponseDto ToDashboardResponseDto(
        RolesDashboard entity)
    {
        return new RolesDashboardResponseDto
        {
            Roleid = entity.Roleid,
            Rolename = entity.Rolename,
            Rolecode = entity.Rolecode,
            Roletype = entity.Roletype,
            Accesslevel = entity.Accesslevel,
            Status = entity.Status,
            Defaultlandingpage = entity.Defaultlandingpage,
            Datascope = entity.Datascope,
            Requiresmfa = entity.Requiresmfa,
            Description = entity.Description,
            Userscount = entity.Userscount ?? 0,
            Createdby = entity.Createdby,
            Createdat = entity.Createdat,
            Updatedby = entity.Updatedby,
            Updatedat = entity.Updatedat
        };
    }
}