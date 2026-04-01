// Services/ConsultationNoteService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ConsultationNoteService : IConsultationNoteService
{
    private readonly TelemedDbContext _context;

    public ConsultationNoteService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<ConsultationNoteResponseDto> CreateAsync(
        CreateConsultationNoteDto dto)
    {
        // Validate Consultation exists
        var consultationExists = await _context.Consultations
            .AnyAsync(c => c.Consultationid == dto.Consultationid);
        if (!consultationExists)
            throw new ArgumentException(
                $"Consultation with ID {dto.Consultationid} does not exist.");

        // Validate Notes not empty
        if (string.IsNullOrWhiteSpace(dto.Notes))
            throw new ArgumentException("Notes cannot be empty.");

        var entity = ConsultationNoteMapper.ToEntity(dto);
        _context.Consultationnotes.Add(entity);
        await _context.SaveChangesAsync();

        var created = await _context.Consultationnotes
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(n => n.Id == entity.Id);

        return ConsultationNoteMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<ConsultationNoteResponseDto>> GetAllAsync()
    {
        var list = await _context.Consultationnotes
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Provider)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(ConsultationNoteMapper.ToResponseDto);
    }

    public async Task<ConsultationNoteResponseDto?> GetByIdAsync(long id)
    {
        var note = await _context.Consultationnotes
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (note == null) return null;
        return ConsultationNoteMapper.ToResponseDto(note);
    }

    public async Task<IEnumerable<ConsultationNoteResponseDto>> GetByConsultationIdAsync(
        long consultationId)
    {
        var list = await _context.Consultationnotes
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Provider)
            .Where(n => n.Consultationid == consultationId)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(ConsultationNoteMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationNoteResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Consultationnotes
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Provider)
            .Where(n => n.Consultation.Patientid == patientId)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(ConsultationNoteMapper.ToResponseDto);
    }

    public async Task<ConsultationNoteResponseDto?> UpdateAsync(
        long id, UpdateConsultationNoteDto dto)
    {
        var entity = await _context.Consultationnotes
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(n => n.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (entity == null) return null;

        if (string.IsNullOrWhiteSpace(dto.Notes))
            throw new ArgumentException("Notes cannot be empty.");

        ConsultationNoteMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ConsultationNoteMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Consultationnotes.FindAsync(id);
        if (entity == null) return false;

        _context.Consultationnotes.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}