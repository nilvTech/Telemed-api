// DTOs/CreatePaymentDto.cs
namespace Telemed.DTOs;

public class CreatePaymentDto
{
    public int Appointmentid { get; set; }
    public int Patientid { get; set; }
    public int Providerid { get; set; }
    public decimal Amount { get; set; }
    public string? Paymentmethod { get; set; }
}