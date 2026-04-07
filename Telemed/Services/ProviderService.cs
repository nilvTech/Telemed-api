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

    // -------------------- GET ALL --------------------
    public async Task<List<ProviderDto>> GetAllAsync()
    {
        var list = await _db.Providers.ToListAsync();
        return list.Select(x => x.ToDto()).ToList();
    }

    // -------------------- GET BY ID --------------------
    public async Task<ProviderDto?> GetByIdAsync(long id)
    {
        var p = await _db.Providers.FindAsync(id);
        return p?.ToDto();
    }

    // -------------------- CREATE --------------------
    public async Task<ProviderDto> CreateAsync(CreateProviderDto dto)
    {
        var entity = dto.ToEntity();
        _db.Providers.Add(entity);
        await _db.SaveChangesAsync();

        return entity.ToDto();
    }

    // -------------------- UPDATE --------------------
    public async Task<ProviderDto?> UpdateAsync(long id, UpdateProviderDto dto)
    {
        var entity = await _db.Providers.FindAsync(id);
        if (entity == null) return null;

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

    // -------------------- DELETE --------------------
    public async Task<bool> DeleteAsync(long id)
    {
        var p = await _db.Providers.FindAsync(id);
        if (p == null) return false;

        _db.Providers.Remove(p);
        await _db.SaveChangesAsync();
        return true;
    }
}