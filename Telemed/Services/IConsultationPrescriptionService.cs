// Services/Interfaces/IConsultationPrescriptionService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IConsultationPrescriptionService
{
    Task<ConsultationPrescriptionResponseDto> CreateAsync(
        CreateConsultationPrescriptionDto dto);
    Task<IEnumerable<ConsultationPrescriptionResponseDto>> GetAllAsync();
    Task<ConsultationPrescriptionResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<ConsultationPrescriptionResponseDto>> GetByConsultationIdAsync(
        long consultationId);
    Task<IEnumerable<ConsultationPrescriptionResponseDto>> GetByPatientIdAsync(
        long patientId);
    Task<IEnumerable<ConsultationPrescriptionResponseDto>> GetByMedicationNameAsync(
        string medicationName);
    Task<ConsultationPrescriptionResponseDto?> UpdateAsync(
        long id, UpdateConsultationPrescriptionDto dto);
    Task<bool> DeleteAsync(long id);
}