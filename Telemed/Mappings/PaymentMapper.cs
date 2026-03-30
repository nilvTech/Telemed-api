// Mappers/PaymentMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class PaymentMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    public static Payment ToEntity(CreatePaymentDto dto)
    {
        return new Payment
        {
            Appointmentid = dto.Appointmentid,
            Patientid = dto.Patientid,
            Providerid = dto.Providerid,
            Amount = dto.Amount,
            Paymentmethod = dto.Paymentmethod,
            Paymentdate = ToUnspecified(DateTime.UtcNow),
            Status = "Pending",
            Createdat = ToUnspecified(DateTime.UtcNow)
        };
    }

    public static void UpdateEntity(Payment entity, UpdatePaymentDto dto)
    {
        if (dto.Amount.HasValue)
            entity.Amount = dto.Amount.Value;

        if (!string.IsNullOrEmpty(dto.Paymentmethod))
            entity.Paymentmethod = dto.Paymentmethod;

        if (!string.IsNullOrEmpty(dto.Status))
            entity.Status = dto.Status;
    }

    public static PaymentResponseDto ToResponseDto(Payment entity)
    {
        return new PaymentResponseDto
        {
            Paymentid = entity.Paymentid,

            // Appointment Info
            Appointmentid = entity.Appointmentid,
            Appointmentdate = entity.Appointment?.Scheduleddatetime,
            Appointmentmode = entity.Appointment?.Mode,

            // Patient Info
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                              ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                                .Replace("  ", " ").Trim()
                              : null,
            Patientemail = entity.Patient?.Email,
            Patientphone = entity.Patient?.Phone,

            // Provider Info
            Providerid = entity.Providerid,
            Providername = entity.Provider?.Providername,
            Speciality = entity.Provider?.Speciality,

            // Payment Details
            Amount = entity.Amount,
            Paymentmethod = entity.Paymentmethod,
            Paymentdate = entity.Paymentdate,
            Status = entity.Status,
            Createdat = entity.Createdat
        };
    }
}