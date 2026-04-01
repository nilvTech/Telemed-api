// Services/ConsultationService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ConsultationService : IConsultationService
{
    private readonly TelemedDbContext _context;

    public ConsultationService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<ConsultationResponseDto> CreateAsync(CreateConsultationDto dto)
    {
        // Validate Appointment exists
        var appointmentExists = await _context.Appointments
            .AnyAsync(a => a.Appointmentid == dto.Appointmentid);
        if (!appointmentExists)
            throw new ArgumentException(
                $"Appointment with ID {dto.Appointmentid} does not exist.");

        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate Provider exists
        var providerExists = await _context.Providers
            .AnyAsync(p => p.Providerid == dto.Providerid);
        if (!providerExists)
            throw new ArgumentException(
                $"Provider with ID {dto.Providerid} does not exist.");

        // Validate vitals if provided
        if (dto.Oxygensaturation.HasValue &&
            (dto.Oxygensaturation < 0 || dto.Oxygensaturation > 100))
            throw new ArgumentException(
                "Oxygen saturation must be between 0 and 100.");

        if (dto.Temperature.HasValue &&
            (dto.Temperature < 90 || dto.Temperature > 115))
            throw new ArgumentException(
                "Temperature must be between 90°F and 115°F.");

        var entity = ConsultationMapper.ToEntity(dto);
        _context.Consultations.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .FirstOrDefaultAsync(c => c.Consultationid == entity.Consultationid);

        return ConsultationMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<ConsultationResponseDto>> GetAllAsync()
    {
        var list = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .Where(c => c.Isactive == true)
            .OrderByDescending(c => c.Createddate)
            .ToListAsync();

        return list.Select(ConsultationMapper.ToResponseDto);
    }

    public async Task<ConsultationResponseDto?> GetByIdAsync(long id)
    {
        var consultation = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .FirstOrDefaultAsync(c => c.Consultationid == id);

        if (consultation == null) return null;
        return ConsultationMapper.ToResponseDto(consultation);
    }

    public async Task<IEnumerable<ConsultationResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .Where(c => c.Patientid == patientId && c.Isactive == true)
            .OrderByDescending(c => c.Createddate)
            .ToListAsync();

        return list.Select(ConsultationMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationResponseDto>> GetByProviderIdAsync(
        long providerId)
    {
        var list = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .Where(c => c.Providerid == providerId && c.Isactive == true)
            .OrderByDescending(c => c.Createddate)
            .ToListAsync();

        return list.Select(ConsultationMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationResponseDto>> GetByAppointmentIdAsync(
        long appointmentId)
    {
        var list = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .Where(c => c.Appointmentid == appointmentId)
            .OrderByDescending(c => c.Createddate)
            .ToListAsync();

        return list.Select(ConsultationMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationResponseDto>> GetByStatusAsync(
        string status)
    {
        var list = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .Where(c => c.Status.ToLower() == status.ToLower()
                     && c.Isactive == true)
            .OrderByDescending(c => c.Createddate)
            .ToListAsync();

        return list.Select(ConsultationMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationResponseDto>> GetFollowUpsAsync(
        long patientId)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var list = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .Where(c => c.Patientid == patientId
                     && c.Followupdate.HasValue
                     && c.Followupdate >= today
                     && c.Isactive == true)
            .OrderBy(c => c.Followupdate)
            .ToListAsync();

        return list.Select(ConsultationMapper.ToResponseDto);
    }

    public async Task<ConsultationResponseDto?> UpdateAsync(
        long id, UpdateConsultationDto dto)
    {
        var entity = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .FirstOrDefaultAsync(c => c.Consultationid == id);

        if (entity == null) return null;

        if (!string.IsNullOrEmpty(dto.Status))
        {
            var validStatuses = new[]
            {
                "InProgress", "Completed", "Cancelled", "OnHold"
            };
            if (!validStatuses.Contains(dto.Status,
                StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException(
                    "Invalid Status. Allowed: InProgress, Completed, Cancelled, OnHold.");
        }

        ConsultationMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ConsultationMapper.ToResponseDto(entity);
    }

    public async Task<ConsultationResponseDto?> UpdateStatusAsync(
        long id, ConsultationStatusUpdateDto dto)
    {
        var entity = await _context.Consultations
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Include(c => c.Appointment)
            .FirstOrDefaultAsync(c => c.Consultationid == id);

        if (entity == null) return null;

        var validStatuses = new[]
        {
            "InProgress", "Completed", "Cancelled", "OnHold"
        };
        if (!validStatuses.Contains(dto.Status,
            StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException(
                "Invalid Status. Allowed: InProgress, Completed, Cancelled, OnHold.");

        entity.Status = dto.Status;
        entity.Updatedby = dto.Updatedby;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return ConsultationMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Consultations.FindAsync(id);
        if (entity == null) return false;

        // Soft delete
        entity.Isactive = false;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();
        return true;
    }
}