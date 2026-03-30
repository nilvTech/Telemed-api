// Services/Interfaces/IPrescriptionService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPrescriptionService
{
    Task<PrescriptionResponseDto> CreateAsync(CreatePrescriptionDto dto);
    Task<IEnumerable<PrescriptionResponseDto>> GetAllAsync();
    Task<PrescriptionResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<PrescriptionResponseDto>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<PrescriptionResponseDto>> GetByEncounterIdAsync(int encounterId);
    Task<IEnumerable<PrescriptionResponseDto>> GetByProviderIdAsync(int providerId);
    Task<PrescriptionResponseDto?> UpdateAsync(int id, UpdatePrescriptionDto dto);
    Task<bool> DeleteAsync(int id);
}