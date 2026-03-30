// Mappers/NotificationMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class NotificationMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Notification ToEntity(CreateNotificationDto dto)
    {
        return new Notification
        {
            Usertype = dto.Usertype,
            Userid = dto.Userid,
            Title = dto.Title,
            Message = dto.Message,
            Isread = false,
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static NotificationResponseDto ToResponseDto(
        Notification entity,
        string? username = null)
    {
        return new NotificationResponseDto
        {
            Notificationid = entity.Notificationid,
            Usertype = entity.Usertype,
            Userid = entity.Userid,
            Username = username,
            Title = entity.Title,
            Message = entity.Message,
            Isread = entity.Isread,
            Createdat = entity.Createdat
        };
    }
}