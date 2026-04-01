// Services/ConsultationPrescriptionService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ConsultationPrescriptionService : IConsultationPrescriptionService
{
    private readonly TelemedDbContext _context;

    public ConsultationPrescriptionService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<ConsultationPrescriptionResponseDto> CreateAsync(
        CreateConsultationPrescriptionDto dto)
    {
        // Validate Consultation exists
        var consultationExists = await _context.Consultations
            .AnyAsync(c => c.Consultationid == dto.Consultationid);
        if (!consultationExists)
            throw new ArgumentException(
                $"Consultation with ID {dto.Consultationid} does not exist.");

        // Validate MedicationName
        if (string.IsNullOrWhiteSpace(dto.Medicationname))
            throw new ArgumentException("Medication name cannot be empty.");

        var entity = ConsultationPrescriptionMapper.ToEntity(dto);
        _context.Consultationprescriptions.Add(entity);

        // Mark consultation as prescription generated
        var consultation = await _context.Consultations
            .FirstOrDefaultAsync(c => c.Consultationid == dto.Consultationid);
        if (consultation != null)
        {
            consultation.Isprescriptiongenerated = true;
            consultation.Updateddate = DateTime.SpecifyKind(
                DateTime.UtcNow, DateTimeKind.Unspecified);
        }

        await _context.SaveChangesAsync();

        var created = await _context.Consultationprescriptions
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(p => p.Prescriptionid == entity.Prescriptionid);

        return ConsultationPrescriptionMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<ConsultationPrescriptionResponseDto>> GetAllAsync()
    {
        var list = await _context.Consultationprescriptions
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Provider)
            .ToListAsync();

        return list.Select(ConsultationPrescriptionMapper.ToResponseDto);
    }

    public async Task<ConsultationPrescriptionResponseDto?> GetByIdAsync(long id)
    {
        var prescription = await _context.Consultationprescriptions
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(p => p.Prescriptionid == id);

        if (prescription == null) return null;
        return ConsultationPrescriptionMapper.ToResponseDto(prescription);
    }

    public async Task<IEnumerable<ConsultationPrescriptionResponseDto>> GetByConsultationIdAsync(
        long consultationId)
    {
        var list = await _context.Consultationprescriptions
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Provider)
            .Where(p => p.Consultationid == consultationId)
            .ToListAsync();

        return list.Select(ConsultationPrescriptionMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationPrescriptionResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Consultationprescriptions
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Provider)
            .Where(p => p.Consultation.Patientid == patientId)
            .OrderByDescending(p => p.Consultation.Createddate)
            .ToListAsync();

        return list.Select(ConsultationPrescriptionMapper.ToResponseDto);
    }

    public async Task<IEnumerable<ConsultationPrescriptionResponseDto>> GetByMedicationNameAsync(
        string medicationName)
    {
        var list = await _context.Consultationprescriptions
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Provider)
            .Where(p => p.Medicationname != null &&
                        p.Medicationname.ToLower()
                         .Contains(medicationName.ToLower()))
            .ToListAsync();

        return list.Select(ConsultationPrescriptionMapper.ToResponseDto);
    }

    public async Task<ConsultationPrescriptionResponseDto?> UpdateAsync(
        long id, UpdateConsultationPrescriptionDto dto)
    {
        var entity = await _context.Consultationprescriptions
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Patient)
            .Include(p => p.Consultation)
                .ThenInclude(c => c.Provider)
            .FirstOrDefaultAsync(p => p.Prescriptionid == id);

        if (entity == null) return null;

        ConsultationPrescriptionMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return ConsultationPrescriptionMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Consultationprescriptions.FindAsync(id);
        if (entity == null) return false;

        _context.Consultationprescriptions.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}