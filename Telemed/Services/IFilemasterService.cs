// Services/Interfaces/IFilemasterService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IFilemasterService
{
    // Core CRUD
    Task<FilemasterResponseDto> CreateAsync(CreateFilemasterDto dto);
    Task<IEnumerable<FilemasterResponseDto>> GetAllAsync();
    Task<FilemasterResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<FilemasterResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<FilemasterResponseDto>> GetCompletedAsync();
    Task<IEnumerable<FilemasterResponseDto>> GetPendingAsync();
    Task<IEnumerable<FilemasterResponseDto>> GetByFileTypeAsync(string filetype);
    Task<FilemasterResponseDto?> UpdateAsync(long id, UpdateFilemasterDto dto);
    Task<bool> DeleteAsync(long id);

    // Chunk Upload — saves each chunk to DB Pdfcontent list
    Task<FilemasterResponseDto?> UploadChunkAsync(UploadChunkDto dto);

    // Direct full file upload — splits into chunks and saves all to DB
    Task<FileUploadResponseDto> UploadRealFileAsync(RealFileUploadDto dto);

    // Download — merges all chunks from DB into single file
    Task<FileDownloadResponseDto?> DownloadFileAsync(long id);

    // Mark complete manually
    Task<FilemasterResponseDto?> MarkCompleteAsync(long id, long? updatedby);
}