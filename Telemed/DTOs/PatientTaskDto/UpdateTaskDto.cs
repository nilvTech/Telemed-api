// DTOs/UpdateTaskDto.cs
namespace Telemed.DTOs;

public class UpdateTaskDto
{
    public string? Taskname { get; set; }
    public DateOnly? Duedate { get; set; }
    public long? Providerinfoid { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? Description { get; set; }
    public DateOnly? Completeddate { get; set; }
    public long? Updatedby { get; set; }
}