// Mappers/MessageMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class MessageMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Message ToEntity(CreateMessageDto dto)
    {
        return new Message
        {
            Patientid = dto.Patientid,
            Providerid = dto.Providerid,
            Sendertype = dto.Sendertype,
            Messagetext = dto.Messagetext,
            Sentat = ToUnspecified(DateTime.UtcNow),
            Isread = false
        };
    }

    public static MessageResponseDto ToResponseDto(Message entity)
    {
        // Resolve sender display name based on SenderType
        string? senderName = entity.Sendertype?.ToLower() switch
        {
            "patient" => entity.Patient != null
                          ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                            .Replace("  ", " ").Trim()
                          : null,
            "provider" => entity.Provider?.Providername,
            _ => null
        };

        return new MessageResponseDto
        {
            Messageid = entity.Messageid,

            // Patient Info
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                           ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                             .Replace("  ", " ").Trim()
                           : null,
            Patientemail = entity.Patient?.Email,
            Patientphone = entity.Patient?.Phone,

            // Provider Info
            Providerid = entity.Providerid,
            Providername = entity.Provider?.Providername,
            Speciality = entity.Provider?.Speciality,

            // Message Details
            Sendertype = entity.Sendertype,
            Sendername = senderName,
            Messagetext = entity.Messagetext,
            Sentat = entity.Sentat,
            Isread = entity.Isread
        };
    }
}