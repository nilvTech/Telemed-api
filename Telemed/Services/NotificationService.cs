using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class NotificationService : INotificationService
{
    private readonly TelemedDbContext _db;

    public NotificationService(TelemedDbContext db)
    {
        _db = db;
    }

    // ============================================================
    // CREATE
    // ============================================================
    public async Task<NotificationResponseDto> CreateAsync(CreateNotificationDto dto)
    {
        var entity = NotificationMapper.ToEntity(dto);

        _db.Notifications.Add(entity);
        await _db.SaveChangesAsync();

        return NotificationMapper.ToResponseDto(entity);
    }

    // ============================================================
    // GET ALL (ADMIN ONLY)
    // ============================================================
    public async Task<IEnumerable<NotificationResponseDto>> GetAllAsync()
    {
        var list = await _db.Notifications
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(n => NotificationMapper.ToResponseDto(n));
    }

    // ============================================================
    // GET BY ID
    // ============================================================
    public async Task<NotificationResponseDto?> GetByIdAsync(int id)
    {
        var entity = await _db.Notifications.FindAsync(id);

        return entity == null ? null : NotificationMapper.ToResponseDto(entity);
    }

    // ============================================================
    // PATIENT FUNCTIONS
    // ============================================================
    public async Task<IEnumerable<NotificationResponseDto>> GetByPatientIdAsync(int patientId)
    {
        var list = await _db.Notifications
            .Where(n => n.Usertype == "PATIENT" && n.Userid == patientId)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(n => NotificationMapper.ToResponseDto(n));
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetUnreadByPatientAsync(int patientId)
    {
        var list = await _db.Notifications
            .Where(n =>
                n.Usertype == "PATIENT" &&
                n.Userid == patientId &&
                n.Isread.GetValueOrDefault() == false)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(n => NotificationMapper.ToResponseDto(n));
    }

    public async Task<bool> MarkAllAsReadByPatientAsync(int patientId)
    {
        var list = await _db.Notifications
            .Where(n =>
                n.Usertype == "PATIENT" &&
                n.Userid == patientId &&
                n.Isread.GetValueOrDefault() == false)
            .ToListAsync();

        if (!list.Any())
            return false;

        foreach (var n in list)
            n.Isread = true;

        await _db.SaveChangesAsync();
        return true;
    }

    // ============================================================
    // PROVIDER FUNCTIONS
    // ============================================================
    public async Task<IEnumerable<NotificationResponseDto>> GetByProviderIdAsync(int providerId)
    {
        var list = await _db.Notifications
            .Where(n => n.Usertype == "PROVIDER" && n.Userid == providerId)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(n => NotificationMapper.ToResponseDto(n));
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetUnreadByProviderAsync(int providerId)
    {
        var list = await _db.Notifications
            .Where(n =>
                n.Usertype == "PROVIDER" &&
                n.Userid == providerId &&
                n.Isread.GetValueOrDefault() == false)
            .OrderByDescending(n => n.Createdat)
            .ToListAsync();

        return list.Select(n => NotificationMapper.ToResponseDto(n));
    }

    public async Task<bool> MarkAllAsReadByProviderAsync(int providerId)
    {
        var list = await _db.Notifications
            .Where(n =>
                n.Usertype == "PROVIDER" &&
                n.Userid == providerId &&
                n.Isread.GetValueOrDefault() == false)
            .ToListAsync();

        if (!list.Any())
            return false;

        foreach (var n in list)
            n.Isread = true;

        await _db.SaveChangesAsync();
        return true;
    }

    // ============================================================
    // MARK SINGLE NOTIFICATION AS READ
    // ============================================================
    public async Task<NotificationResponseDto?> MarkAsReadAsync(int id)
    {
        var entity = await _db.Notifications.FindAsync(id);

        if (entity == null)
            return null;

        entity.Isread = true;
        await _db.SaveChangesAsync();

        return NotificationMapper.ToResponseDto(entity);
    }

    // ============================================================
    // DELETE
    // ============================================================
    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _db.Notifications.FindAsync(id);

        if (entity == null)
            return false;

        _db.Notifications.Remove(entity);
        await _db.SaveChangesAsync();

        return true;
    }
}