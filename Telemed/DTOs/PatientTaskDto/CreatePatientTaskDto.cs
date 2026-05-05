// DTOs/CreateTaskDto.cs
namespace Telemed.DTOs;

public class CreatePatientTaskDto
{
    public string Taskname { get; set; } = null!;
    public DateOnly Duedate { get; set; }
    public long Patientid { get; set; }
    public long? Providerinfoid { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? Description { get; set; }
    public long? Createdby { get; set; }
}