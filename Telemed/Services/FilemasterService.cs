// Services/FilemasterService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class FilemasterService : IFilemasterService
{
    private readonly TelemedDbContext _context;

    public FilemasterService(TelemedDbContext context)
    {
        _context = context;
    }

    // Helper to get patient
    private async Task<Patient?> GetPatientAsync(long? patientId)
    {
        if (!patientId.HasValue) return null;
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.Patientid == patientId.Value);
    }

    public async Task<FilemasterResponseDto> CreateAsync(CreateFilemasterDto dto)
    {
        // Validate Patient exists if provided
        if (dto.Patientid.HasValue)
        {
            var patientExists = await _context.Patients
                .AnyAsync(p => p.Patientid == dto.Patientid);
            if (!patientExists)
                throw new ArgumentException(
                    $"Patient with ID {dto.Patientid} does not exist.");
        }

        // Validate Filename
        if (string.IsNullOrWhiteSpace(dto.Filename))
            throw new ArgumentException("Filename cannot be empty.");

        // Validate FileType
        var validFileTypes = new[]
        {
            "pdf", "jpg", "jpeg", "png", "gif",
            "doc", "docx", "xls", "xlsx",
            "mp4", "mov", "avi",
            "dicom", "dcm", "hl7", "xml", "txt",
            "zip", "csv"
        };
        if (!validFileTypes.Contains(dto.Filetype.ToLower()))
            throw new ArgumentException(
                "Invalid file type. Allowed: pdf, jpg, jpeg, png, gif, " +
                "doc, docx, xls, xlsx, mp4, mov, avi, " +
                "dicom, dcm, hl7, xml, txt, zip, csv.");

        // Validate file size (max 500MB)
        if (dto.Totalsize <= 0)
            throw new ArgumentException("File size must be greater than 0.");

        if (dto.Totalsize > 500L * 1024 * 1024)
            throw new ArgumentException(
                "File size cannot exceed 500MB.");

        // Validate chunks
        if (dto.Totalchunks <= 0)
            throw new ArgumentException("Total chunks must be greater than 0.");

        var entity = FilemasterMapper.ToEntity(dto);
        _context.Filemasters.Add(entity);
        await _context.SaveChangesAsync();

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetAllAsync()
    {
        var list = await _context.Filemasters
            .OrderByDescending(f => f.Createddate)
            .ToListAsync();

        var result = new List<FilemasterResponseDto>();
        foreach (var entity in list)
        {
            var patient = await GetPatientAsync(entity.Patientid);
            result.Add(FilemasterMapper.ToResponseDto(entity, patient));
        }
        return result;
    }

    public async Task<FilemasterResponseDto?> GetByIdAsync(long id)
    {
        var entity = await _context.Filemasters
            .FirstOrDefaultAsync(f => f.Fileid == id);

        if (entity == null) return null;

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await _context.Filemasters
            .Where(f => f.Patientid == patientId)
            .OrderByDescending(f => f.Createddate)
            .ToListAsync();

        var patient = await GetPatientAsync(patientId);

        return list.Select(f => FilemasterMapper.ToResponseDto(f, patient));
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetCompletedAsync()
    {
        var list = await _context.Filemasters
            .Where(f => f.Iscompleted == true)
            .OrderByDescending(f => f.Createddate)
            .ToListAsync();

        var result = new List<FilemasterResponseDto>();
        foreach (var entity in list)
        {
            var patient = await GetPatientAsync(entity.Patientid);
            result.Add(FilemasterMapper.ToResponseDto(entity, patient));
        }
        return result;
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetPendingAsync()
    {
        var list = await _context.Filemasters
            .Where(f => f.Iscompleted == false)
            .OrderByDescending(f => f.Createddate)
            .ToListAsync();

        var result = new List<FilemasterResponseDto>();
        foreach (var entity in list)
        {
            var patient = await GetPatientAsync(entity.Patientid);
            result.Add(FilemasterMapper.ToResponseDto(entity, patient));
        }
        return result;
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetByFileTypeAsync(
        string filetype)
    {
        var list = await _context.Filemasters
            .Where(f => f.Filetype.ToLower() == filetype.ToLower())
            .OrderByDescending(f => f.Createddate)
            .ToListAsync();

        var result = new List<FilemasterResponseDto>();
        foreach (var entity in list)
        {
            var patient = await GetPatientAsync(entity.Patientid);
            result.Add(FilemasterMapper.ToResponseDto(entity, patient));
        }
        return result;
    }

    public async Task<FilemasterResponseDto?> UpdateAsync(
        long id, UpdateFilemasterDto dto)
    {
        var entity = await _context.Filemasters
            .FirstOrDefaultAsync(f => f.Fileid == id);

        if (entity == null) return null;

        FilemasterMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    // Track chunk upload progress
    public async Task<FilemasterResponseDto?> UpdateChunkAsync(UploadChunkDto dto)
    {
        var entity = await _context.Filemasters
            .FirstOrDefaultAsync(f => f.Fileid == dto.Fileid);

        if (entity == null) return null;

        if (entity.Iscompleted == true)
            throw new ArgumentException("File upload is already completed.");

        // Validate chunk number
        if (dto.Chunknumber <= 0 || dto.Chunknumber > entity.Totalchunks)
            throw new ArgumentException(
                $"Invalid chunk number. Must be between 1 and {entity.Totalchunks}.");

        // Increment uploaded chunks
        entity.Uploadedchunks = (entity.Uploadedchunks ?? 0) + 1;

        // Auto mark complete if all chunks uploaded
        if (entity.Uploadedchunks >= entity.Totalchunks)
            entity.Iscompleted = true;

        entity.Updatedby = dto.Updatedby;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    // Manually mark file upload as complete
    public async Task<FilemasterResponseDto?> MarkCompleteAsync(
        long id, long? updatedby)
    {
        var entity = await _context.Filemasters
            .FirstOrDefaultAsync(f => f.Fileid == id);

        if (entity == null) return null;

        entity.Iscompleted = true;
        entity.Uploadedchunks = entity.Totalchunks;
        entity.Updatedby = updatedby;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Filemasters.FindAsync(id);
        if (entity == null) return false;

        _context.Filemasters.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    // Add to FilemasterService.cs
    public async Task<FileUploadResponseDto> UploadRealFileAsync(RealFileUploadDto dto)
    {
        // Validate file provided
        if (dto.File == null || dto.File.Length == 0)
            throw new ArgumentException("No file provided.");

        // Validate Patient if provided
        Patient? patient = null;
        if (dto.Patientid.HasValue)
        {
            patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.Patientid == dto.Patientid);
            if (patient == null)
                throw new ArgumentException(
                    $"Patient with ID {dto.Patientid} does not exist.");
        }

        // Validate file type
        var extension = Path.GetExtension(dto.File.FileName)
            .TrimStart('.').ToLower();
        var validFileTypes = new[]
        {
        "pdf", "jpg", "jpeg", "png", "gif",
        "doc", "docx", "xls", "xlsx",
        "mp4", "mov", "avi",
        "dicom", "dcm", "hl7", "xml", "txt",
        "zip", "csv"
    };
        if (!validFileTypes.Contains(extension))
            throw new ArgumentException(
                $"Invalid file type '{extension}'. " +
                "Allowed: pdf, jpg, jpeg, png, gif, doc, docx, " +
                "xls, xlsx, mp4, dicom, txt, zip, csv.");

        // Validate file size max 500MB
        if (dto.File.Length > 500L * 1024 * 1024)
            throw new ArgumentException("File size cannot exceed 500MB.");

        // Create upload directory
        var uploadFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads",
            "PatientFiles",
            dto.Patientid?.ToString() ?? "general");

        Directory.CreateDirectory(uploadFolder);

        // Generate unique filename to avoid conflicts
        var uniqueFilename = $"{Guid.NewGuid()}_{dto.File.FileName}";
        var filePath = Path.Combine(uploadFolder, uniqueFilename);

        // Save file to disk
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.File.CopyToAsync(stream);
        }

        // Calculate chunks (1MB per chunk)
        var chunkSize = 1024 * 1024; // 1MB
        var totalChunks = (int)Math.Ceiling(
            (double)dto.File.Length / chunkSize);

        // Create Filemaster record
        var entity = new Filemaster
        {
            Patientid = dto.Patientid,
            Filename = dto.File.FileName,
            Filetype = extension,
            Totalsize = dto.File.Length,
            Totalchunks = totalChunks,
            Uploadedchunks = totalChunks,   // All chunks done — real upload
            Iscompleted = true,           // File fully uploaded
            Createdby = dto.Createdby,
            Createddate = DateTime.SpecifyKind(
                DateTime.UtcNow, DateTimeKind.Unspecified),
            Updateddate = DateTime.SpecifyKind(
                DateTime.UtcNow, DateTimeKind.Unspecified)
        };

        _context.Filemasters.Add(entity);
        await _context.SaveChangesAsync();

        // Format size
        string sizeFormatted;
        if (entity.Totalsize < 1024)
            sizeFormatted = $"{entity.Totalsize} B";
        else if (entity.Totalsize < 1024 * 1024)
            sizeFormatted = $"{entity.Totalsize / 1024.0:F1} KB";
        else
            sizeFormatted = $"{entity.Totalsize / (1024.0 * 1024):F1} MB";

        return new FileUploadResponseDto
        {
            Fileid = entity.Fileid,
            Patientname = patient != null
                                    ? $"{patient.Firstname} {patient.Middlename} {patient.Lastname}"
                                      .Replace("  ", " ").Trim()
                                    : null,
            Mrn = patient?.Mrn,
            Filename = entity.Filename,
            Filetype = entity.Filetype,
            Totalsize = entity.Totalsize,
            Filesizeformatted = sizeFormatted,
            Totalchunks = entity.Totalchunks,
            Uploadedchunks = entity.Uploadedchunks ?? 0,
            Uploadprogresspercent = 100,
            Iscompleted = true,
            Savedpath = filePath,
            Createddate = entity.Createddate
        };
    }
}