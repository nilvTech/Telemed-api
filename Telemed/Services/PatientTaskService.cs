using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PatientTaskService : IPatientTaskService
{
    private readonly TelemedDbContext _context;

    public PatientTaskService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<PatientTask> BaseQuery()
    {
        return _context.PatientTasks
            .Include(t => t.Patient)
            .Include(t => t.Providerinfo);
    }

    public async Task<TaskResponseDto> CreateAsync(CreatePatientTaskDto dto)
    {
        var entity = PatientTaskMapper.ToEntity(dto);

        _context.PatientTasks.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstAsync(x => x.Taskid == entity.Taskid);

        return PatientTaskMapper.ToResponseDto(created);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery().ToListAsync();
        return list.Select(PatientTaskMapper.ToResponseDto);
    }

    public async Task<TaskResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(x => x.Taskid == id);

        return entity == null ? null : PatientTaskMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetByPatientIdAsync(long patientId)
    {
        var list = await BaseQuery()
            .Where(x => x.Patientid == patientId)
            .ToListAsync();

        return list.Select(PatientTaskMapper.ToResponseDto);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetByProviderIdAsync(long providerId)
    {
        var list = await BaseQuery()
            .Where(x => x.Providerinfoid == providerId)
            .ToListAsync();

        return list.Select(PatientTaskMapper.ToResponseDto);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetByStatusAsync(string status)
    {
        var list = await BaseQuery()
            .Where(x => x.Status == status)
            .ToListAsync();

        return list.Select(PatientTaskMapper.ToResponseDto);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetByPriorityAsync(string priority)
    {
        var list = await BaseQuery()
            .Where(x => x.Priority == priority)
            .ToListAsync();

        return list.Select(PatientTaskMapper.ToResponseDto);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.PatientTasks.FindAsync(id);
        if (entity == null) return false;

        _context.PatientTasks.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // Optional (simple versions)
    public async Task<IEnumerable<TaskResponseDto>> GetOverdueAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await BaseQuery()
            .Where(x => x.Duedate < today)
            .ToListAsync();

        return list.Select(PatientTaskMapper.ToResponseDto);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetDueTodayAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await BaseQuery()
            .Where(x => x.Duedate == today)
            .ToListAsync();

        return list.Select(PatientTaskMapper.ToResponseDto);
    }

    public async Task<TaskResponseDto?> UpdateAsync(long id, UpdateTaskDto dto)
    {
        var entity = await _context.PatientTasks.FindAsync(id);
        if (entity == null) return null;

        entity.Taskname = dto.Taskname ?? entity.Taskname;
        entity.Status = dto.Status ?? entity.Status;
        entity.Priority = dto.Priority ?? entity.Priority;
        entity.Description = dto.Description ?? entity.Description;
        entity.Updatedat = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return PatientTaskMapper.ToResponseDto(entity);
    }

    public async Task<TaskResponseDto?> UpdateStatusAsync(long id, TaskStatusUpdateDto dto)
    {
        var entity = await _context.PatientTasks.FindAsync(id);
        if (entity == null) return null;

        entity.Status = dto.Status;
        entity.Updatedat = DateTime.UtcNow;

        if (dto.Status == "Completed")
            entity.Completeddate = DateOnly.FromDateTime(DateTime.Today);

        await _context.SaveChangesAsync();

        return PatientTaskMapper.ToResponseDto(entity);
    }
}