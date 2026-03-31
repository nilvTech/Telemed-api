// DTOs/CreatePaymentDto.cs
namespace Telemed.DTOs;

public class CreatePaymentDto
{
    public int Appointmentid { get; set; }
    public long Patientid { get; set; }
    public long Providerid { get; set; }
    public decimal Amount { get; set; }
    public string? Paymentmethod { get; set; }
}