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

    // 1MB per chunk
    private const int ChunkSize = 1024 * 1024;

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

    // Helper to validate file type
    private static readonly string[] ValidFileTypes = new[]
    {
        "pdf", "jpg", "jpeg", "png", "gif",
        "doc", "docx", "xls", "xlsx",
        "mp4", "mov", "avi",
        "dicom", "dcm", "hl7", "xml", "txt",
        "zip", "csv"
    };

    private static string FormatSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024):F1} MB";
    }

    public async Task<FilemasterResponseDto> CreateAsync(CreateFilemasterDto dto)
    {
        // Validate Patient
        if (dto.Patientid.HasValue)
        {
            var exists = await _context.Patients
                .AnyAsync(p => p.Patientid == dto.Patientid);
            if (!exists)
                throw new ArgumentException(
                    $"Patient with ID {dto.Patientid} does not exist.");
        }

        if (string.IsNullOrWhiteSpace(dto.Filename))
            throw new ArgumentException("Filename cannot be empty.");

        if (!ValidFileTypes.Contains(dto.Filetype.ToLower()))
            throw new ArgumentException(
                $"Invalid file type. Allowed: {string.Join(", ", ValidFileTypes)}.");

        if (dto.Totalsize <= 0)
            throw new ArgumentException("File size must be greater than 0.");

        if (dto.Totalsize > 500L * 1024 * 1024)
            throw new ArgumentException("File size cannot exceed 500MB.");

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

    // ✅ Upload single chunk — saves chunk bytes to Pdfcontent list in DB
    public async Task<FilemasterResponseDto?> UploadChunkAsync(UploadChunkDto dto)
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

        // Validate chunk file provided
        if (dto.Chunkdata == null || dto.Chunkdata.Length == 0)
            throw new ArgumentException("Chunk data cannot be empty.");

        // Read chunk bytes
        using var memoryStream = new MemoryStream();
        await dto.Chunkdata.CopyToAsync(memoryStream);
        var chunkBytes = memoryStream.ToArray();

        // Initialize list if null
        entity.Pdfcontent ??= new List<byte[]>();

        // Add chunk bytes to Pdfcontent list
        entity.Pdfcontent.Add(chunkBytes);

        // Update chunk count
        entity.Uploadedchunks = entity.Pdfcontent.Count;

        // Auto complete when all chunks received
        if (entity.Uploadedchunks >= entity.Totalchunks)
            entity.Iscompleted = true;

        entity.Updatedby = dto.Updatedby;
        entity.Updateddate = DateTime.SpecifyKind(
            DateTime.UtcNow, DateTimeKind.Unspecified);

        await _context.SaveChangesAsync();

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    // ✅ Full file upload — reads file, splits into chunks, saves all to DB
    public async Task<FileUploadResponseDto> UploadRealFileAsync(
        RealFileUploadDto dto)
    {
        // Validate file
        if (dto.File == null || dto.File.Length == 0)
            throw new ArgumentException("No file provided.");

        // Validate Patient
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
        if (!ValidFileTypes.Contains(extension))
            throw new ArgumentException(
                $"Invalid file type '{extension}'. " +
                $"Allowed: {string.Join(", ", ValidFileTypes)}.");

        // Validate file size max 500MB
        if (dto.File.Length > 500L * 1024 * 1024)
            throw new ArgumentException("File size cannot exceed 500MB.");

        // Read entire file into memory
        using var fileStream = new MemoryStream();
        await dto.File.CopyToAsync(fileStream);
        var fileBytes = fileStream.ToArray();

        // Split file into chunks of 1MB each
        var chunks = new List<byte[]>();
        var totalBytes = fileBytes.Length;
        var offset = 0;

        while (offset < totalBytes)
        {
            var remainingBytes = totalBytes - offset;
            var currentChunkSize = Math.Min(ChunkSize, remainingBytes);
            var chunk = new byte[currentChunkSize];
            Array.Copy(fileBytes, offset, chunk, 0, currentChunkSize);
            chunks.Add(chunk);
            offset += currentChunkSize;
        }

        // Create Filemaster record with all chunks stored
        var entity = new Filemaster
        {
            Patientid = dto.Patientid,
            Filename = dto.File.FileName,
            Filetype = extension,
            Totalsize = dto.File.Length,
            Totalchunks = chunks.Count,
            Uploadedchunks = chunks.Count,  // All chunks uploaded
            Iscompleted = true,           // Fully uploaded
            Pdfcontent = chunks,         // ✅ All chunks stored in DB
            Createdby = dto.Createdby,
            Createddate = DateTime.SpecifyKind(
                DateTime.UtcNow, DateTimeKind.Unspecified),
            Updateddate = DateTime.SpecifyKind(
                DateTime.UtcNow, DateTimeKind.Unspecified)
        };

        _context.Filemasters.Add(entity);
        await _context.SaveChangesAsync();

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
            Filesizeformatted = FormatSize(entity.Totalsize),
            Totalchunks = entity.Totalchunks,
            Uploadedchunks = entity.Uploadedchunks ?? 0,
            Uploadprogresspercent = 100,
            Iscompleted = true,
            Storedchunkcount = chunks.Count,
            Createddate = entity.Createddate
        };
    }

    // ✅ Download — merges all chunks from DB into single byte array
    public async Task<FileDownloadResponseDto?> DownloadFileAsync(long id)
    {
        var entity = await _context.Filemasters
            .FirstOrDefaultAsync(f => f.Fileid == id);

        if (entity == null) return null;

        if (entity.Iscompleted == false)
            throw new ArgumentException(
                "File upload is not yet completed. Cannot download.");

        if (entity.Pdfcontent == null || entity.Pdfcontent.Count == 0)
            throw new ArgumentException(
                "No file data found in database for this record.");

        // Merge all chunks into single byte array
        var totalSize = entity.Pdfcontent.Sum(c => c.Length);
        var mergedFile = new byte[totalSize];
        var position = 0;

        foreach (var chunk in entity.Pdfcontent)
        {
            Array.Copy(chunk, 0, mergedFile, position, chunk.Length);
            position += chunk.Length;
        }

        return new FileDownloadResponseDto
        {
            Fileid = entity.Fileid,
            Filename = entity.Filename,
            Filetype = entity.Filetype,
            Filedata = mergedFile,
            Totalsize = entity.Totalsize,
            Filesizeformatted = FormatSize(entity.Totalsize)
        };
    }

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
}