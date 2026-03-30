// Services/Interfaces/IMessageService.cs
using Telemed.DTOs;

namespace Telemed.Services.Interfaces;

public interface IMessageService
{
    Task<MessageResponseDto> CreateAsync(CreateMessageDto dto);
    Task<IEnumerable<MessageResponseDto>> GetAllAsync();
    Task<MessageResponseDto?> GetByIdAsync(int id);
    Task<IEnumerable<MessageResponseDto>> GetConversationAsync(int patientId, int providerId);
    Task<IEnumerable<MessageResponseDto>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<MessageResponseDto>> GetByProviderIdAsync(int providerId);
    Task<IEnumerable<MessageResponseDto>> GetUnreadAsync(int patientId, int providerId);
    Task<MessageResponseDto?> MarkAsReadAsync(int id);
    Task<bool> MarkAllAsReadAsync(int patientId, int providerId);
    Task<bool> DeleteAsync(int id);
}