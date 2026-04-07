// Services/Interfaces/IFilemasterService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IFilemasterService
{
    Task<FilemasterResponseDto> CreateAsync(CreateFilemasterDto dto);
    Task<IEnumerable<FilemasterResponseDto>> GetAllAsync();
    Task<FilemasterResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<FilemasterResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<FilemasterResponseDto>> GetCompletedAsync();
    Task<IEnumerable<FilemasterResponseDto>> GetPendingAsync();
    Task<IEnumerable<FilemasterResponseDto>> GetByFileTypeAsync(string filetype);
    Task<FilemasterResponseDto?> UpdateAsync(long id, UpdateFilemasterDto dto);
    Task<FilemasterResponseDto?> UpdateChunkAsync(UploadChunkDto dto);
    Task<FilemasterResponseDto?> MarkCompleteAsync(long id, long? updatedby);
    Task<bool> DeleteAsync(long id);
    // Add to IFilemasterService.cs
    Task<FileUploadResponseDto> UploadRealFileAsync(RealFileUploadDto dto);
}