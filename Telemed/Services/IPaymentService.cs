// Services/Interfaces/IPaymentService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto);
    Task<IEnumerable<PaymentResponseDto>> GetAllAsync();
    Task<PaymentResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<PaymentResponseDto>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<PaymentResponseDto>> GetByProviderIdAsync(int providerId);
    Task<IEnumerable<PaymentResponseDto>> GetByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<PaymentResponseDto>> GetByStatusAsync(string status);
    Task<PaymentResponseDto?> UpdateAsync(int id, UpdatePaymentDto dto);
    Task<bool> DeleteAsync(int id);
}