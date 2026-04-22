// Services/Interfaces/IFollowupService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IFollowupService
{
    Task<FollowupResponseDto> CreateAsync(CreateFollowupDto dto);
    Task<IEnumerable<FollowupResponseDto>> GetAllAsync();
    Task<FollowupResponseDto?> GetByIdAsync(long id);
    Task<IEnumerable<FollowupResponseDto>> GetByPatientIdAsync(long patientId);
    Task<IEnumerable<FollowupResponseDto>> GetByAppointmentIdAsync(long appointmentId);
    Task<IEnumerable<FollowupResponseDto>> GetByStatusAsync(string status);
    Task<IEnumerable<FollowupResponseDto>> GetByTypeAsync(string followuptype);
    Task<IEnumerable<FollowupResponseDto>> GetOverdueAsync();
    Task<IEnumerable<FollowupResponseDto>> GetTodayAsync();
    Task<IEnumerable<FollowupResponseDto>> GetUpcomingAsync(int days);
    Task<FollowupResponseDto?> UpdateAsync(long id, UpdateFollowupDto dto);
    Task<bool> DeleteAsync(long id);
}