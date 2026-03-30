// Services/Interfaces/IEncounterService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IEncounterService
{
    Task<EncounterResponseDto> CreateAsync(CreateEncounterDto dto);
    Task<IEnumerable<EncounterResponseDto>> GetAllAsync();
    Task<EncounterResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<EncounterResponseDto>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<EncounterResponseDto>> GetByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<EncounterResponseDto>> GetByProviderIdAsync(int providerId);
    Task<EncounterResponseDto?> UpdateAsync(int id, UpdateEncounterDto dto);
    Task<bool> DeleteAsync(int id);
}