using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IPatientService
{
    Task<List<PatientDto>> GetAllAsync();
    Task<PatientDto?> GetByIdAsync(long id);
    Task<PatientDto> CreateAsync(CreatePatientDto dto);
    Task<PatientDto?> UpdateAsync(long id, UpdatePatientDto dto);
    Task<bool> DeleteAsync(long id);
}