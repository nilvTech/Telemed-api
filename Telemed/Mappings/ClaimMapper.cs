// Mappers/ClaimMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ClaimMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Claim ToEntity(CreateClaimDto dto)
    {
        // Auto calculate total amount from details
        var totalAmount = dto.Claimdetails
            .Sum(d => d.Amount * (d.Units ?? 1));

        return new Claim
        {
            Patientid = dto.Patientid,
            Providerinfoid = dto.Providerinfoid,
            Payer = dto.Payer,
            Claimnumber = dto.Claimnumber,
            Totaltime = dto.Totaltime,
            Totalamount = totalAmount,
            Status = "Pending",
            Icdcode = dto.Icdcode,
            Submissiondate = dto.Submissiondate,
            Paidamount = 0,
            Createdby = dto.Createdby,
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static Claimdetail ToDetailEntity(
        CreateClaimDetailDto dto, long claimid)
    {
        return new Claimdetail
        {
            Claimid = claimid,
            Cptcode = dto.Cptcode,
            Description = dto.Description,
            Units = dto.Units ?? 1,
            Amount = dto.Amount
        };
    }

    public static void UpdateEntity(Claim entity, UpdateClaimDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Payer))
            entity.Payer = dto.Payer;

        if (dto.Totaltime.HasValue)
            entity.Totaltime = dto.Totaltime;

        if (!string.IsNullOrEmpty(dto.Icdcode))
            entity.Icdcode = dto.Icdcode;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (dto.Submissiondate.HasValue)
            entity.Submissiondate = dto.Submissiondate;

        if (dto.Paidamount.HasValue)
            entity.Paidamount = dto.Paidamount;

        if (!string.IsNullOrEmpty(dto.Deniedreason))
            entity.Deniedreason = dto.Deniedreason;

        entity.Updatedby = dto.Updatedby;
        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static void UpdateDetailEntity(
        Claimdetail entity, UpdateClaimDetailDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Cptcode))
            entity.Cptcode = dto.Cptcode;

        if (!string.IsNullOrEmpty(dto.Description))
            entity.Description = dto.Description;

        if (dto.Units.HasValue)
            entity.Units = dto.Units;

        if (dto.Amount.HasValue)
            entity.Amount = dto.Amount.Value;
    }

    public static ClaimDetailResponseDto ToDetailResponseDto(
        Claimdetail entity)
    {
        return new ClaimDetailResponseDto
        {
            Claimdetailid = entity.Claimdetailid,
            Claimid = entity.Claimid,
            Cptcode = entity.Cptcode,
            Description = entity.Description,
            Units = entity.Units,
            Amount = entity.Amount
        };
    }

    public static ClaimResponseDto ToResponseDto(Claim entity)
    {
        var balance = entity.Totalamount - (entity.Paidamount ?? 0);

        return new ClaimResponseDto
        {
            Claimid = entity.Claimid,

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

            // Claim
            Payer = entity.Payer,
            Claimnumber = entity.Claimnumber,
            Totaltime = entity.Totaltime,
            Totalamount = entity.Totalamount,
            Status = entity.Status,
            Submissiondate = entity.Submissiondate,
            Paidamount = entity.Paidamount,
            Balancedue = balance,
            Deniedreason = entity.Deniedreason,
            Icdcode = entity.Icdcode,

            Claimdetails = entity.Claimdetails
                                 .Select(ToDetailResponseDto)
                                 .ToList(),

            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat
        };
    }
}