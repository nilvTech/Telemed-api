// Services/AppointmentDocumentService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class AppointmentDocumentService : IAppointmentDocumentService
{
    private readonly TelemedDbContext _context;

    public AppointmentDocumentService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<AppointmentDocumentResponseDto> CreateAsync(
        CreateAppointmentDocumentDto dto)
    {
        // Validate Appointment exists
        var appointmentExists = await _context.Appointments
            .AnyAsync(a => a.Appointmentid == dto.Appointmentid);
        if (!appointmentExists)
            throw new ArgumentException(
                $"Appointment with ID {dto.Appointmentid} does not exist.");

        // Validate FileUrl
        if (string.IsNullOrWhiteSpace(dto.Fileurl))
            throw new ArgumentException("File URL cannot be empty.");

        // Validate FileType
        if (!string.IsNullOrEmpty(dto.Filetype))
        {
            var validFileTypes = new[]
            {
                "pdf", "jpg", "jpeg", "png",
                "doc", "docx", "xls", "xlsx",
                "dicom", "hl7", "xml", "txt"
            };
            if (!validFileTypes.Contains(dto.Filetype.ToLower()))
                throw new ArgumentException(
                    "Invalid file type. Allowed: pdf, jpg, jpeg, png, " +
                    "doc, docx, xls, xlsx, dicom, hl7, xml, txt.");
        }

        var entity = AppointmentDocumentMapper.ToEntity(dto);
        _context.Appointmentdocuments.Add(entity);
        await _context.SaveChangesAsync();

        // Reload with related data
        var created = await _context.Appointmentdocuments
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Provider)
            .FirstOrDefaultAsync(d => d.Id == entity.Id);

        return AppointmentDocumentMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<AppointmentDocumentResponseDto>> GetAllAsync()
    {
        var list = await _context.Appointmentdocuments
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Provider)
            .OrderByDescending(d => d.Uploadedat)
            .ToListAsync();

        return list.Select(AppointmentDocumentMapper.ToResponseDto);
    }

    public async Task<AppointmentDocumentResponseDto?> GetByIdAsync(long id)
    {
        var document = await _context.Appointmentdocuments
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Provider)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document == null) return null;
        return AppointmentDocumentMapper.ToResponseDto(document);
    }

    public async Task<IEnumerable<AppointmentDocumentResponseDto>> GetByAppointmentIdAsync(
        long appointmentId)
    {
        var list = await _context.Appointmentdocuments
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Provider)
            .Where(d => d.Appointmentid == appointmentId)
            .OrderByDescending(d => d.Uploadedat)
            .ToListAsync();

        return list.Select(AppointmentDocumentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentDocumentResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Appointmentdocuments
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Provider)
            .Where(d => d.Appointment != null &&
                        d.Appointment.Patientid == patientId)
            .OrderByDescending(d => d.Uploadedat)
            .ToListAsync();

        return list.Select(AppointmentDocumentMapper.ToResponseDto);
    }

    public async Task<IEnumerable<AppointmentDocumentResponseDto>> GetByFileTypeAsync(
        string filetype)
    {
        var list = await _context.Appointmentdocuments
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Provider)
            .Where(d => d.Filetype != null &&
                        d.Filetype.ToLower() == filetype.ToLower())
            .OrderByDescending(d => d.Uploadedat)
            .ToListAsync();

        return list.Select(AppointmentDocumentMapper.ToResponseDto);
    }

    public async Task<AppointmentDocumentResponseDto?> UpdateAsync(
        long id, UpdateAppointmentDocumentDto dto)
    {
        var entity = await _context.Appointmentdocuments
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Patient)
            .Include(d => d.Appointment)
                .ThenInclude(a => a!.Provider)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (entity == null) return null;

        AppointmentDocumentMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return AppointmentDocumentMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Appointmentdocuments.FindAsync(id);
        if (entity == null) return false;

        _context.Appointmentdocuments.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}