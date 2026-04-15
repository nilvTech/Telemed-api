using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPatientTaskService
{
    Task<TaskResponseDto> CreateAsync(CreatePatientTaskDto dto);

    Task<IEnumerable<TaskResponseDto>> GetAllAsync();

    Task<TaskResponseDto?> GetByIdAsync(long id);

    Task<IEnumerable<TaskResponseDto>> GetByPatientIdAsync(long patientId);

    Task<IEnumerable<TaskResponseDto>> GetByProviderIdAsync(long providerId);

    Task<IEnumerable<TaskResponseDto>> GetByStatusAsync(string status);

    Task<IEnumerable<TaskResponseDto>> GetByPriorityAsync(string priority);

    Task<IEnumerable<TaskResponseDto>> GetOverdueAsync();

    Task<IEnumerable<TaskResponseDto>> GetDueTodayAsync();

    Task<TaskResponseDto?> UpdateAsync(long id, UpdateTaskDto dto);

    Task<TaskResponseDto?> UpdateStatusAsync(long id, TaskStatusUpdateDto dto);

    Task<bool> DeleteAsync(long id);
}