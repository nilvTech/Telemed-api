// Services/ConsultationDiagnosisService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ConsultationDiagnosisService : IConsultationDiagnosisService
{
    private readonly TelemedDbContext _context;

    public ConsultationDiagnosisService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<ConsultationDiagnosisResponseDto> CreateAsync(
        CreateConsultationDiagnosisDto dto)
    {
        // Validate Consultation exists
        var consultationExists = await _context.Consultations
            .AnyAsync(c => c.Consultationid == dto.Consultationid);
        if (!consultationExists)
            throw new ArgumentException(
                $"Consultation with ID {dto.Consultationid} does not exist.");

        // Validate at least one field provided
        if (string.IsNullOrWhiteSpace(dto.Diagnosiscode) &&
            string.IsNullOrWhiteSpace(dto.Diagnosisname))
            throw new ArgumentException(
                "At least one of DiagnosisCode or DiagnosisName must be provided.");

        var entity = ConsultationDiagnosisMapper.ToEntity(dto);
        _context.Consultationdiagnoses.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.Consultationdiagnoses
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(d => d.Id == entity.Id);

        return ConsultationDiagnosisMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<ConsultationDiagnosisResponseDto>> GetAllAsync()
    {
        var list = await _context.Consultationdiagnoses
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Provider)
            .ToListAsync();

        return list.Select(ConsultationDiagnosisMapper.ToResponseDto);
    }

    public async Task<ConsultationDiagnosisResponseDto?> GetByIdAsync(long id)
    {
        var diagnosis = await _context.Consultationdiagnoses
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (diagnosis == null) return null;
        return ConsultationDiagnosisMapper.ToResponseDto(diagnosis);
    }

    public async Task<IEnumerable<ConsultationDiagnosisResponseDto>> GetByConsultationIdAsync(
        long consultationId)
    {
        var list = await _context.Consultationdiagnoses
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Provider)
            .Where(d => d.Consultationid == consultationId)
            .ToListAsync();

        return list.Select(ConsultationDiagnosisMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationDiagnosisResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Consultationdiagnoses
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Provider)
            .Where(d => d.Consultation.Patientid == patientId)
            .OrderByDescending(d => d.Consultation.Createddate)
            .ToListAsync();

        return list.Select(ConsultationDiagnosisMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationDiagnosisResponseDto>> GetByDiagnosisCodeAsync(
        string code)
    {
        var list = await _context.Consultationdiagnoses
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Provider)
            .Where(d => d.Diagnosiscode != null &&
                        d.Diagnosiscode.ToLower() == code.ToLower())
            .ToListAsync();

        return list.Select(ConsultationDiagnosisMapper.ToResponseDto);
    }

    public async Task<ConsultationDiagnosisResponseDto?> UpdateAsync(
        long id, UpdateConsultationDiagnosisDto dto)
    {
        var entity = await _context.Consultationdiagnoses
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(d => d.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null) return null;

        ConsultationDiagnosisMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ConsultationDiagnosisMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Consultationdiagnoses.FindAsync(id);
        if (entity == null) return false;

        _context.Consultationdiagnoses.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}