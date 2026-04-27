// Services/Interfaces/ICareteampatientService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface ICareteampatientService
{
    Task<CareteampatientResponseDto> CreateAsync(CreateCareteampatientDto dto);
    Task<IEnumerable<CareteampatientResponseDto>> GetAllAsync();
    Task<CareteampatientResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<CareteampatientResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<CareteampatientResponseDto>> GetByCareteamIdAsync(long careteamId);
    Task<IEnumerable<CareteampatientResponseDto>> GetActiveAsync();
    Task<IEnumerable<CareteampatientResponseDto>> GetActiveByCareteamIdAsync(long careteamId);
    Task<IEnumerable<CareteampatientResponseDto>> GetActiveByPatientIdAsync(long patientId);
    Task<CareteampatientResponseDto?> UpdateAsync(long id, UpdateCareteampatientDto dto);
    Task<bool> DeactivateAsync(long id);
    Task<bool> DeleteAsync(long id);
}