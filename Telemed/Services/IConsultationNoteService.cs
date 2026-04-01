// Services/Interfaces/IConsultationNoteService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IConsultationNoteService
{
    Task<ConsultationNoteResponseDto> CreateAsync(CreateConsultationNoteDto dto);
    Task<IEnumerable<ConsultationNoteResponseDto>> GetAllAsync();
    Task<ConsultationNoteResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<ConsultationNoteResponseDto>> GetByConsultationIdAsync(
        long consultationId);
    Task<IEnumerable<ConsultationNoteResponseDto>> GetByPatientIdAsync(
        long patientId);
    Task<ConsultationNoteResponseDto?> UpdateAsync(
        long id, UpdateConsultationNoteDto dto);
    Task<bool> DeleteAsync(long id);
}