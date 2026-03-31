// DTOs/PaymentResponseDto.cs
namespace Telemed.DTOs;

public class PaymentResponseDto
{
    public long Paymentid { get; set; }

    // Appointment Info
    public long Appointmentid { get; set; }
    public DateTime? Appointmentdate { get; set; }
    public string? Appointmentmode { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Patientemail { get; set; }
    public string? Patientphone { get; set; }

    // Provider Info
    public long Providerid { get; set; }
    public string? Providername { get; set; }
    public string? Speciality { get; set; }

    // Payment Details
    public decimal Amount { get; set; }
    public string? Paymentmethod { get; set; }
    public DateTime? Paymentdate { get; set; }
    public string? Status { get; set; }
    public DateTime? Createdat { get; set; }
}