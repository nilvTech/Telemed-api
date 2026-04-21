using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Telemed.Services;

public class FilemasterService : IFilemasterService
{
    private readonly TelemedDbContext _context;

    private const int ChunkSize = 1024 * 1024;

    public FilemasterService(TelemedDbContext context)
    {
        _context = context;
    }

    private async Task<Patient?> GetPatientAsync(long? patientId)
    {
        if (!patientId.HasValue) return null; 
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.Patientid == patientId.Value);
    }

    private static readonly string[] ValidFileTypes = new[]
    {
        "pdf","jpg","jpeg","png","gif",
        "doc","docx","xls","xlsx",
        "mp4","mov","avi",
        "dicom","dcm","hl7","xml","txt",
        "zip","csv"
    };

    private static string FormatSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024):F1} MB";
    }

    public async Task<FilemasterResponseDto> CreateAsync(CreateFilemasterDto dto)
    {
        if (dto.Patientid.HasValue)
        {
            var exists = await _context.Patients
                .AnyAsync(p => p.Patientid == dto.Patientid);
            if (!exists)
                throw new ArgumentException($"Patient with ID {dto.Patientid} does not exist.");
        }

        if (string.IsNullOrWhiteSpace(dto.Filename))
            throw new ArgumentException("Filename cannot be empty.");

        if (!ValidFileTypes.Contains(dto.Filetype.ToLower()))
            throw new ArgumentException("Invalid file type.");

        var entity = FilemasterMapper.ToEntity(dto);
        _context.Filemasters.Add(entity);
        await _context.SaveChangesAsync();

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetAllAsync()
    {
        var list = await _context.Filemasters.ToListAsync();
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
        var entity = await _context.Filemasters.FirstOrDefaultAsync(f => f.Fileid == id);
        if (entity == null) return null;

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetByPatientIdAsync(long patientId)
    {
        var list = await _context.Filemasters
            .Where(f => f.Patientid == patientId)
            .ToListAsync();

        var patient = await GetPatientAsync(patientId);
        return list.Select(f => FilemasterMapper.ToResponseDto(f, patient));
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetCompletedAsync()
    {
        var list = await _context.Filemasters
            .Where(f => f.Iscompleted == true)
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
            .ToListAsync();

        var result = new List<FilemasterResponseDto>();
        foreach (var entity in list)
        {
            var patient = await GetPatientAsync(entity.Patientid);
            result.Add(FilemasterMapper.ToResponseDto(entity, patient));
        }

        return result;
    }

    public async Task<IEnumerable<FilemasterResponseDto>> GetByFileTypeAsync(string filetype)
    {
        var list = await _context.Filemasters
            .Where(f => f.Filetype.ToLower() == filetype.ToLower())
            .ToListAsync();

        var result = new List<FilemasterResponseDto>();
        foreach (var entity in list)
        {
            var patient = await GetPatientAsync(entity.Patientid);
            result.Add(FilemasterMapper.ToResponseDto(entity, patient));
        }

        return result;
    }

    public async Task<FilemasterResponseDto?> UpdateAsync(long id, UpdateFilemasterDto dto)
    {
        var entity = await _context.Filemasters.FirstOrDefaultAsync(f => f.Fileid == id);
        if (entity == null) return null;

        FilemasterMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    public async Task<FilemasterResponseDto?> UploadChunkAsync(UploadChunkDto dto)
    {
        var entity = await _context.Filemasters.FirstOrDefaultAsync(f => f.Fileid == dto.Fileid);
        if (entity == null) return null;

        using var memoryStream = new MemoryStream();
        await dto.Chunkdata.CopyToAsync(memoryStream);
        var chunkBytes = memoryStream.ToArray();

        entity.Pdfcontent ??= new List<byte[]>();
        entity.Pdfcontent.Add(chunkBytes);
        entity.Uploadedchunks = entity.Pdfcontent.Count;

        if (entity.Uploadedchunks >= entity.Totalchunks)
            entity.Iscompleted = true;

        await _context.SaveChangesAsync();

        var patient = await GetPatientAsync(entity.Patientid);
        return FilemasterMapper.ToResponseDto(entity, patient);
    }

    public async Task<FileUploadResponseDto> UploadRealFileAsync(RealFileUploadDto dto)
    {
        using var ms = new MemoryStream();
        await dto.File.CopyToAsync(ms);

        var entity = new Filemaster
        {
            Patientid = dto.Patientid,
            Filename = dto.File.FileName,
            Filetype = Path.GetExtension(dto.File.FileName).TrimStart('.'),
            Totalsize = dto.File.Length,
            Totalchunks = 1,
            Uploadedchunks = 1,
            Iscompleted = true,
            Pdfcontent = new List<byte[]> { ms.ToArray() },
            Createddate = DateTime.UtcNow
        };

        _context.Filemasters.Add(entity);
        await _context.SaveChangesAsync();

        return new FileUploadResponseDto
        {
            Fileid = entity.Fileid,
            Filename = entity.Filename
        };
    }

    public async Task<FileDownloadResponseDto?> DownloadFileAsync(long id)
    {
        var entity = await _context.Filemasters.FirstOrDefaultAsync(f => f.Fileid == id);
        if (entity == null || entity.Pdfcontent == null) return null;

        var merged = entity.Pdfcontent.SelectMany(c => c).ToArray();

        return new FileDownloadResponseDto
        {
            Fileid = entity.Fileid,
            Filename = entity.Filename,
            Filedata = merged
        };
    }

    public async Task<FilemasterResponseDto?> MarkCompleteAsync(long id, long? updatedby)
    {
        var entity = await _context.Filemasters.FirstOrDefaultAsync(f => f.Fileid == id);
        if (entity == null) return null;

        entity.Iscompleted = true;
        entity.Uploadedchunks = entity.Totalchunks;
        entity.Updatedby = updatedby;
        entity.Updateddate = DateTime.UtcNow;

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

    // ========================= NEW METHODS (ERROR FIX) =========================

    public async Task<Filemaster> UploadOrderFileAsync(long clinicalOrderId, IFormFile file)
    {
        var orderExists = await _context.Clinicalorders
            .AnyAsync(x => x.Clinicalorderid == clinicalOrderId);

        if (!orderExists)
            throw new Exception("Invalid Clinical Order Id");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var entity = new Filemaster
        {
            Clinicalorderid = clinicalOrderId,
            Filename = file.FileName,
            Filetype = Path.GetExtension(file.FileName).TrimStart('.'),
            Totalsize = ms.Length,
            Totalchunks = 1,
            Uploadedchunks = 1,
            Iscompleted = true,
            Createddate = DateTime.UtcNow,
            Pdfcontent = new List<byte[]> { ms.ToArray() }
        };

        _context.Filemasters.Add(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<Filemaster>> GetFilesByOrderIdAsync(long clinicalOrderId)
    {
        return await _context.Filemasters
            .Where(x => x.Clinicalorderid == clinicalOrderId)
            .ToListAsync();
    }
}