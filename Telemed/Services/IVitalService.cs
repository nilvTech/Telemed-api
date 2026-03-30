// Services/Interfaces/IVitalService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IVitalService
{
    Task<VitalResponseDto> CreateAsync(CreateVitalDto dto);
    Task<IEnumerable<VitalResponseDto>> GetAllAsync();
    Task<VitalResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<VitalResponseDto>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<VitalResponseDto>> GetByEncounterIdAsync(int encounterId);
    Task<VitalResponseDto?> UpdateAsync(int id, UpdateVitalDto dto);
    Task<bool> DeleteAsync(int id);
}