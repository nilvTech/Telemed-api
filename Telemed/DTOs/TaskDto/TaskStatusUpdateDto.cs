// DTOs/TaskStatusUpdateDto.cs
namespace Telemed.DTOs;

public class TaskStatusUpdateDto
{
    public string Status { get; set; } = null!;
    public DateOnly? Completeddate { get; set; }
    public long? Updatedby { get; set; }
}