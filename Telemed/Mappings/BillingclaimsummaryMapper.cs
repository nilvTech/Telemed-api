// Mappers/BillingclaimsummaryMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class BillingclaimsummaryMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Billingclaimsummary ToEntity(
        CreateBillingclaimsummaryDto dto)
    {
        return new Billingclaimsummary
        {
            Claimid = dto.Claimid,
            Patientid = dto.Patientid,
            Providerinfoid = dto.Providerinfoid,
            Cptcode = dto.Cptcode,
            Amount = dto.Amount,
            Status = dto.Status,
            Claimdate = ToUnspecified(dto.Claimdate),
            Createdat = ToUnspecified(DateTime.UtcNow),
            Updatedat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(
        Billingclaimsummary entity,
        UpdateBillingclaimsummaryDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Cptcode))
            entity.Cptcode = dto.Cptcode;

        if (dto.Amount.HasValue)
            entity.Amount = dto.Amount.Value;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;

        if (dto.Claimdate.HasValue)
            entity.Claimdate = ToUnspecified(dto.Claimdate.Value);

        entity.Updatedat = ToUnspecified(DateTime.UtcNow);
    }

    public static BillingclaimsummaryResponseDto ToResponseDto(
        Billingclaimsummary entity)
    {
        return new BillingclaimsummaryResponseDto
        {
            Summaryid = entity.Summaryid,

            // Claim
            Claimid = entity.Claimid,
            Claimnumber = entity.Claim?.Claimnumber,
            Claimpayer = entity.Claim?.Payer,
            Claimtotalamount = entity.Claim?.Totalamount,

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
            NpiNumber = entity.Providerinfo?.Providerprofile?.NpiNumber,

            // Billing
            Cptcode = entity.Cptcode,
            Amount = entity.Amount,
            Status = entity.Status,
            Claimdate = entity.Claimdate,
            Createdat = entity.Createdat,
            Updatedat = entity.Updatedat
        };
    }
}