// DTOs/CreateAppointmentDocumentDto.cs
namespace Telemed.DTOs;

public class CreateAppointmentDocumentDto
{
    public long Appointmentid { get; set; }
    public string Fileurl { get; set; } = null!;
    public string? Filetype { get; set; }
}