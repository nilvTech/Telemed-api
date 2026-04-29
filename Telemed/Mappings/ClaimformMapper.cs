// Mappers/ClaimformMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class ClaimformMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    private static int? CalculateAge(DateOnly? dob)
    {
        if (!dob.HasValue) return null;
        var today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - dob.Value.Year;
        if (dob.Value > today.AddYears(-age)) age--;
        return age;
    }

    public static Claimform ToEntity(CreateClaimformDto dto)
    {
        // Auto calculate total from service amount
        var total = dto.Serviceamount ?? 0;

        return new Claimform
        {
            Patientname = dto.Patientname,
            Dateofbirth = dto.Dateofbirth,
            Gender = dto.Gender,
            Phone = dto.Phone,
            Address = dto.Address,
            Insuranceplan = dto.Insuranceplan,
            Policynumber = dto.Policynumber,
            Insuredname = dto.Insuredname,
            Dateofillness = dto.Dateofillness,
            Referringprovider = dto.Referringprovider,
            Diagnosiscode = dto.Diagnosiscode,
            Servicedate = dto.Servicedate,
            Servicecptcode = dto.Servicecptcode,
            Servicedescription = dto.Servicedescription,
            Serviceamount = dto.Serviceamount,
            Totalamount = total,
            Createdby = dto.Createdby,
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Claimform entity, UpdateClaimformDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Patientname))
            entity.Patientname = dto.Patientname;

        if (!string.IsNullOrEmpty(dto.Phone))
            entity.Phone = dto.Phone;

        if (!string.IsNullOrEmpty(dto.Address))
            entity.Address = dto.Address;

        if (!string.IsNullOrEmpty(dto.Insuranceplan))
            entity.Insuranceplan = dto.Insuranceplan;

        if (!string.IsNullOrEmpty(dto.Policynumber))
            entity.Policynumber = dto.Policynumber;

        if (!string.IsNullOrEmpty(dto.Insuredname))
            entity.Insuredname = dto.Insuredname;

        if (dto.Dateofillness.HasValue)
            entity.Dateofillness = dto.Dateofillness;

        if (!string.IsNullOrEmpty(dto.Referringprovider))
            entity.Referringprovider = dto.Referringprovider;

        if (!string.IsNullOrEmpty(dto.Diagnosiscode))
            entity.Diagnosiscode = dto.Diagnosiscode;

        if (dto.Servicedate.HasValue)
            entity.Servicedate = dto.Servicedate;

        if (!string.IsNullOrEmpty(dto.Servicecptcode))
            entity.Servicecptcode = dto.Servicecptcode;

        if (!string.IsNullOrEmpty(dto.Servicedescription))
            entity.Servicedescription = dto.Servicedescription;

        if (dto.Serviceamount.HasValue)
        {
            entity.Serviceamount = dto.Serviceamount;
            // Recalculate total
            entity.Totalamount = dto.Serviceamount;
        }
    }

    public static ClaimformResponseDto ToResponseDto(Claimform entity)
    {
        return new ClaimformResponseDto
        {
            Claimformid = entity.Claimformid,

            // Patient
            Patientname = entity.Patientname,
            Dateofbirth = entity.Dateofbirth,
            Age = CalculateAge(entity.Dateofbirth),
            Gender = entity.Gender,
            Phone = entity.Phone,
            Address = entity.Address,

            // Insurance
            Insuranceplan = entity.Insuranceplan,
            Policynumber = entity.Policynumber,
            Insuredname = entity.Insuredname,

            // Claim
            Dateofillness = entity.Dateofillness,
            Referringprovider = entity.Referringprovider,
            Diagnosiscode = entity.Diagnosiscode,

            // Service
            Servicedate = entity.Servicedate,
            Servicecptcode = entity.Servicecptcode,
            Servicedescription = entity.Servicedescription,
            Serviceamount = entity.Serviceamount,

            // Total
            Totalamount = entity.Totalamount,

            Createdat = entity.Createdat,
            Createdby = entity.Createdby
        };
    }
}