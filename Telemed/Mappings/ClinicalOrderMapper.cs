// Mappers/ClinicalOrderMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ClinicalOrderMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    // ===== ClinicalMaster =====

    public static Clinicalmaster ToMasterEntity(CreateClinicalMasterDto dto)
    {
        return new Clinicalmaster
        {
            Ordertype = dto.Ordertype,
            Ordername = dto.Ordername,
            Ordercode = dto.Ordercode,
            Description = dto.Description,
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateMasterEntity(
        Clinicalmaster entity,
        UpdateClinicalMasterDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Ordername))
            entity.Ordername = dto.Ordername;

        if (!string.IsNullOrEmpty(dto.Description))
            entity.Description = dto.Description;
    }

    public static ClinicalMasterResponseDto ToMasterResponseDto(
        Clinicalmaster entity)
    {
        return new ClinicalMasterResponseDto
        {
            Clinicalmasterid = entity.Clinicalmasterid,
            Ordertype = entity.Ordertype,
            Ordername = entity.Ordername,
            Ordercode = entity.Ordercode,
            Description = entity.Description,
            TotalOrders = entity.Clinicalorders.Count,
            Createdat = entity.Createdat
        };
    }

    // ===== ClinicalOrder =====

    public static Clinicalorder ToOrderEntity(CreateClinicalOrderDto dto)
    {
        return new Clinicalorder
        {
            Patientid = dto.Patientid,
            Encounterid = dto.Encounterid,
            Providerinfoid = dto.Providerinfoid,
            Clinicalmasterid = dto.Clinicalmasterid,
            Priority = dto.Priority ?? "Routine",
            Status = "Pending",
            Orderdate = ToUnspecified(DateTime.UtcNow),
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateOrderEntity(
        Clinicalorder entity,
        UpdateClinicalOrderDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Priority))
            entity.Priority = dto.Priority;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (dto.Completeddate.HasValue)
            entity.Completeddate = ToUnspecified(dto.Completeddate.Value);

        // Auto set completed date
        if (dto.Status?.ToLower() == "completed" &&
            !entity.Completeddate.HasValue)
            entity.Completeddate = ToUnspecified(DateTime.UtcNow);

        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static ClinicalOrderResponseDto ToOrderResponseDto(
        Clinicalorder entity)
    {
        return new ClinicalOrderResponseDto
        {
            Clinicalorderid = entity.Clinicalorderid,

            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                               ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                                 .Replace("  ", " ").Trim()
                               : null,
            Mrn = entity.Patient?.Mrn,

            // Provider
            Providerinfoid = entity.Providerinfoid,
            Providername = entity.Providerinfo != null
                               ? $"{entity.Providerinfo.Firstname} {entity.Providerinfo.Lastname}"
                                 .Trim()
                               : null,
            Providerspeciality = entity.Providerinfo?.Providerprofile?.Speciality1,

            // Encounter
            Encounterid = entity.Encounterid,
            Encounterdate = entity.Encounter?.Encounterdate,

            // Master Info
            Clinicalmasterid = entity.Clinicalmasterid,
            Ordertype = entity.Clinicalmaster?.Ordertype,
            Ordername = entity.Clinicalmaster?.Ordername,
            Ordercode = entity.Clinicalmaster?.Ordercode,
            Orderdescription = entity.Clinicalmaster?.Description,

            // Order Details
            Priority = entity.Priority,
            Status = entity.Status,
            IsUrgent = entity.Priority == "STAT" ||
                               entity.Priority == "Urgent",
            Orderdate = entity.Orderdate,
            Completeddate = entity.Completeddate,
            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat


            // File Master


        };
    }
}