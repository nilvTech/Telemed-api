using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappings;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class ProviderService : IProviderService
{
    private readonly TelemedDbContext _db;

    public ProviderService(TelemedDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProviderDto>> GetAllAsync()
    {
        var list = await _db.Providers.ToListAsync();
        return list.Select(x => x.ToDto()).ToList();
    }

    public async Task<ProviderDto?> GetByIdAsync(int id)
    {
        var p = await _db.Providers.FindAsync(id);
        return p?.ToDto();
    }

    public async Task<ProviderDto> CreateAsync(CreateProviderDto dto)
    {
        var entity = dto.ToEntity();
        _db.Providers.Add(entity);
        await _db.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<ProviderDto?> UpdateAsync(int id, UpdateProviderDto dto)
    {
        var entity = await _db.Providers.FindAsync(id);
        if (entity == null)
            return null;

        // Update fields
        entity.Providername = dto.ProviderName;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.Speciality = dto.Speciality;
        entity.Website = dto.Website;
        entity.Primaryaddress = dto.PrimaryAddress;
        entity.Status = dto.Status;
        entity.Updatedat = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var p = await _db.Providers.FindAsync(id);
        if (p == null)
            return false;

        _db.Providers.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
}