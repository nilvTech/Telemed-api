// Services/CareteampatientService.cs
using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class CareteampatientService : ICareteampatientService
{
    private readonly TelemedDbContext _context;

    public CareteampatientService(TelemedDbContext context)
    {
        _context = context;
    }

    private IQueryable<Careteampatient> BaseQuery()
    {
        return _context.Careteampatients
            .Include(c => c.Patient)
            .Include(c => c.Careteam);
    }

    public async Task<CareteampatientResponseDto> CreateAsync(
        CreateCareteampatientDto dto)
    {
        // Validate Patient exists
        var patientExists = await _context.Patients
            .AnyAsync(p => p.Patientid == dto.Patientid);
        if (!patientExists)
            throw new ArgumentException(
                $"Patient with ID {dto.Patientid} does not exist.");

        // Validate Careteam exists
        var careteamExists = await _context.Careteams
            .AnyAsync(c => c.Careteamid == dto.Careteamid);
        if (!careteamExists)
            throw new ArgumentException(
                $"Care team with ID {dto.Careteamid} does not exist.");

        // Validate not already active member
        var alreadyAssigned = await _context.Careteampatients
            .AnyAsync(c => c.Patientid == dto.Patientid &&
                           c.Careteamid == dto.Careteamid &&
                           c.Isactive == true);
        if (alreadyAssigned)
            throw new ArgumentException(
                "Patient is already an active member of this care team.");

        var entity = CareteampatientMapper.ToEntity(dto);
        _context.Careteampatients.Add(entity);
        await _context.SaveChangesAsync();

        var created = await BaseQuery()
            .FirstOrDefaultAsync(c =>
                c.Careteampatientid == entity.Careteampatientid);

        return CareteampatientMapper.ToResponseDto(created!);
    }

    public async Task<IEnumerable<CareteampatientResponseDto>> GetAllAsync()
    {
        var list = await BaseQuery()
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteampatientMapper.ToResponseDto);
    }

    public async Task<CareteampatientResponseDto?> GetByIdAsync(long id)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careteampatientid == id);

        if (entity == null) return null;
        return CareteampatientMapper.ToResponseDto(entity);
    }

    public async Task<IEnumerable<CareteampatientResponseDto>> GetByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(c => c.Patientid == patientId)
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteampatientMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteampatientResponseDto>> GetByCareteamIdAsync(
        long careteamId)
    {
        var list = await BaseQuery()
            .Where(c => c.Careteamid == careteamId)
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteampatientMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteampatientResponseDto>> GetActiveAsync()
    {
        var list = await BaseQuery()
            .Where(c => c.Isactive == true)
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteampatientMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteampatientResponseDto>> GetActiveByCareteamIdAsync(
        long careteamId)
    {
        var list = await BaseQuery()
            .Where(c => c.Careteamid == careteamId &&
                        c.Isactive == true)
            .OrderBy(c => c.Patient.Lastname)
            .ToListAsync();

        return list.Select(CareteampatientMapper.ToResponseDto);
    }

    public async Task<IEnumerable<CareteampatientResponseDto>> GetActiveByPatientIdAsync(
        long patientId)
    {
        var list = await BaseQuery()
            .Where(c => c.Patientid == patientId &&
                        c.Isactive == true)
            .OrderByDescending(c => c.Assigneddate)
            .ToListAsync();

        return list.Select(CareteampatientMapper.ToResponseDto);
    }

    public async Task<CareteampatientResponseDto?> UpdateAsync(
        long id, UpdateCareteampatientDto dto)
    {
        var entity = await BaseQuery()
            .FirstOrDefaultAsync(c => c.Careteampatientid == id);

        if (entity == null) return null;

        CareteampatientMapper.UpdateEntity(entity, dto);
        await _context.SaveChangesAsync();
        return CareteampatientMapper.ToResponseDto(entity);
    }

    public async Task<bool> DeactivateAsync(long id)
    {
        var entity = await _context.Careteampatients
            .FirstOrDefaultAsync(c => c.Careteampatientid == id);

        if (entity == null) return false;

        entity.Isactive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _context.Careteampatients.FindAsync(id);
        if (entity == null) return false;

        _context.Careteampatients.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}