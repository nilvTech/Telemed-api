// Mappers/FilemasterMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class FilemasterMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    // Format file size to human readable
    private static string FormatFileSize(long bytes)
    {
        if (bytes < 1024)
            return $"{bytes} B";
        else if (bytes < 1024 * 1024)
            return $"{bytes / 1024.0:F1} KB";
        else if (bytes < 1024 * 1024 * 1024)
            return $"{bytes / (1024.0 * 1024):F1} MB";
        else
            return $"{bytes / (1024.0 * 1024 * 1024):F2} GB";
    }

    // Calculate upload progress percentage
    private static int? CalculateProgress(int? uploaded, int total)
    {
        if (!uploaded.HasValue || total == 0)
            return 0;
        return (int)Math.Round((double)uploaded.Value / total * 100);
    }

    public static Filemaster ToEntity(CreateFilemasterDto dto)
    {
        return new Filemaster
        {
            Patientid = dto.Patientid,
            Filename = dto.Filename,
            Filetype = dto.Filetype,
            Totalsize = dto.Totalsize,
            Totalchunks = dto.Totalchunks,
            Uploadedchunks = 0,
            Iscompleted = false,
            Createdby = dto.Createdby,
            Createddate = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Filemaster entity, UpdateFilemasterDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Filename))
            entity.Filename = dto.Filename;

        if (dto.Uploadedchunks.HasValue)
        {
            entity.Uploadedchunks = dto.Uploadedchunks;

            // Auto mark complete if all chunks uploaded
            if (entity.Uploadedchunks >= entity.Totalchunks)
                entity.Iscompleted = true;
        }

        if (dto.Iscompleted.HasValue)
            entity.Iscompleted = dto.Iscompleted;

        entity.Updatedby = dto.Updatedby;
        entity.Updateddate = ToUnspecified(DateTime.UtcNow);
    }

    public static FilemasterResponseDto ToResponseDto(
        Filemaster entity,
        Patient? patient = null)
    {
        return new FilemasterResponseDto
        {
            Fileid = entity.Fileid,

            // Patient Info
            Patientid = entity.Patientid,
            Patientname = patient != null
                                     ? $"{patient.Firstname} {patient.Middlename} {patient.Lastname}"
                                       .Replace("  ", " ").Trim()
                                     : null,
            Mrn = patient?.Mrn,

            // File Details
            Filename = entity.Filename,
            Filetype = entity.Filetype,
            Totalsize = entity.Totalsize,
            Filesizeformatted = FormatFileSize(entity.Totalsize),
            Totalchunks = entity.Totalchunks,
            Uploadedchunks = entity.Uploadedchunks,
            Uploadprogresspercent = CalculateProgress(
                                        entity.Uploadedchunks,
                                        entity.Totalchunks),
            Iscompleted = entity.Iscompleted,

            // Audit
            Createddate = entity.Createddate,
            Createdby = entity.Createdby,
            Updatedby = entity.Updatedby,
            Updateddate = entity.Updateddate
        };
    }
}