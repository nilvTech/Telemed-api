// Services/MessageService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class MessageService : IMessageService
{
    private readonly TelemedDbContext _context;

    public MessageService(TelemedDbContext context)
    {
        _context = context;
    }

    public async Task<MessageResponseDto> CreateAsync(CreateMessageDto dto)
    {
        // Validate SenderType
        var validSenderTypes = new[] { "Patient", "Provider" };
        if (!validSenderTypes.Contains(dto.Sendertype, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("SenderType must be either 'Patient' or 'Provider'.");

        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException($"Patient with ID {dto.Patientid} does not exist.");

        // Validate Provider exists
        var providerExists = await _context.Providers
            .AnyAsync(p => p.Providerid == dto.Providerid);
        if (!providerExists)
            throw new ArgumentException($"Provider with ID {dto.Providerid} does not exist.");

        // Validate Message is not empty
        if (string.IsNullOrWhiteSpace(dto.Messagetext))
            throw new ArgumentException("Message text cannot be empty.");

        var entity = MessageMapper.ToEntity(dto);
        _context.Messages.Add(entity);
        await _context.SaveChangesAsync();

        // Reload with related data for full response
        var created = await _context.Messages
            .Include(m => m.Patient)
            .Include(m => m.Provider)
            .FirstOrDefaultAsync(m => m.Messageid == entity.Messageid);

        return MessageMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<MessageResponseDto>> GetAllAsync()
    {
        var list = await _context.Messages
            .Include(m => m.Patient)
            .Include(m => m.Provider)
            .OrderByDescending(m => m.Sentat)
            .ToListAsync();

        return list.Select(MessageMapper.ToResponseDto);
    }

    public async Task<MessageResponseDto?> GetByIdAsync(int id)
    {
        var message = await _context.Messages
            .Include(m => m.Patient)
            .Include(m => m.Provider)
            .FirstOrDefaultAsync(m => m.Messageid == id);

        if (message == null) return null;
        return MessageMapper.ToResponseDto(message);
    }

    // Full conversation thread between patient and provider
    public async Task<IEnumerable<MessageResponseDto>> GetConversationAsync(
        int patientId, int providerId)
    {
        var list = await _context.Messages
            .Include(m => m.Patient)
            .Include(m => m.Provider)
            .Where(m => m.Patientid == patientId
                     && m.Providerid == providerId)
            .OrderBy(m => m.Sentat)
            .ToListAsync();

        return list.Select(MessageMapper.ToResponseDto);
    }

    public async Task<IEnumerable<MessageResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var list = await _context.Messages
            .Include(m => m.Patient)
            .Include(m => m.Provider)
            .Where(m => m.Patientid == patientId)
            .OrderByDescending(m => m.Sentat)
            .ToListAsync();

        return list.Select(MessageMapper.ToResponseDto);
    }

    public async Task<IEnumerable<MessageResponseDto>> GetByProviderIdAsync(int providerId)
    {
        var list = await _context.Messages
            .Include(m => m.Patient)
            .Include(m => m.Provider)
            .Where(m => m.Providerid == providerId)
            .OrderByDescending(m => m.Sentat)
            .ToListAsync();

        return list.Select(MessageMapper.ToResponseDto);
    }

    // Get unread messages in a conversation
    public async Task<IEnumerable<MessageResponseDto>> GetUnreadAsync(
        int patientId, int providerId)
    {
        var list = await _context.Messages
            .Include(m => m.Patient)
            .Include(m => m.Provider)
            .Where(m => m.Patientid == patientId
                     && m.Providerid == providerId
                     && m.Isread == false)
            .OrderBy(m => m.Sentat)
            .ToListAsync();

        return list.Select(MessageMapper.ToResponseDto);
    }

    // Mark single message as read
    public async Task<MessageResponseDto?> MarkAsReadAsync(int id)
    {
        var entity = await _context.Messages
            .Include(m => m.Patient)
            .Include(m => m.Provider)
            .FirstOrDefaultAsync(m => m.Messageid == id);

        if (entity == null) return null;

        entity.Isread = true;
        await _context.SaveChangesAsync();
        return MessageMapper.ToResponseDto(entity);
    }

    // Mark all messages in a conversation as read
    public async Task<bool> MarkAllAsReadAsync(int patientId, int providerId)
    {
        var messages = await _context.Messages
            .Where(m => m.Patientid == patientId
                     && m.Providerid == providerId
                     && m.Isread == false)
            .ToListAsync();

        if (!messages.Any()) return false;

        messages.ForEach(m => m.Isread = true);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Messages.FindAsync(id);
        if (entity == null) return false;

        _context.Messages.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}