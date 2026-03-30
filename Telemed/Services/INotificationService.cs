using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface INotificationService
{
    Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto);
    Task<IEnumerable<NotificationResponseDto>> GetAllAsync();
    Task<NotificationResponseDto?> GetByIdAsync(int id);

    // --- PATIENT FUNCTIONS ---
    Task<IEnumerable<NotificationResponseDto>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<NotificationResponseDto>> GetUnreadByPatientAsync(int patientId);
    Task<bool> MarkAllAsReadByPatientAsync(int patientId);

    // --- PROVIDER FUNCTIONS ---
    Task<IEnumerable<NotificationResponseDto>> GetByProviderIdAsync(int providerId);
    Task<IEnumerable<NotificationResponseDto>> GetUnreadByProviderAsync(int providerId);
    Task<bool> MarkAllAsReadByProviderAsync(int providerId);

    // --- COMMON FUNCTIONS ---
    Task<NotificationResponseDto?> MarkAsReadAsync(int id);
    Task<bool> DeleteAsync(int id);
}