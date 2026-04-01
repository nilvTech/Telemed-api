// Services/Interfaces/IConsultationService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IConsultationService
{
    Task<ConsultationResponseDto> CreateAsync(CreateConsultationDto dto);
    Task<IEnumerable<ConsultationResponseDto>> GetAllAsync();
    Task<ConsultationResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<ConsultationResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<ConsultationResponseDto>> GetByProviderIdAsync(long providerId);
    Task<IEnumerable<ConsultationResponseDto>> GetByAppointmentIdAsync(long appointmentId);
    Task<IEnumerable<ConsultationResponseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<ConsultationResponseDto>> GetFollowUpsAsync(long patientId);
    Task<ConsultationResponseDto?> UpdateAsync(long id, UpdateConsultationDto dto);
    Task<ConsultationResponseDto?> UpdateStatusAsync(long id, ConsultationStatusUpdateDto dto);
    Task<bool> DeleteAsync(long id);
}