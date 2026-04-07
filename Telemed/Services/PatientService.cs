using Microsoft.EntityFrameworkCore;
using Telemed.Models;
using Telemed.DTOs;
using Telemed.Services.Interfaces;

namespace Telemed.Services;

public class PatientService : IPatientService
{
    private readonly TelemedDbContext _db;

    public PatientService(TelemedDbContext db)
    {
        _db = db;
    }

    // -------------------- HELPERS --------------------
    private DateTime? ToDateTime(DateOnly? d) =>
        d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : null;

    private DateOnly? ToDateOnly(DateTime? d) =>
        d.HasValue ? DateOnly.FromDateTime(d.Value) : null;

    private PatientDto MapToDto(Patient x) => new PatientDto
    {
        PatientId = x.Patientid,
        FirstName = x.Firstname,
        LastName = x.Lastname,
        MiddleName = x.Middlename,
        Gender = x.Gender,
        Email = x.Email,
        Phone = x.Phone,
        DateOfBirth = ToDateTime(x.Dateofbirth),
        Address = x.Address,
        Language = x.Language,
        MaritalStatus = x.Maritalstatus,
        MRN = x.Mrn
    };

    // -------------------- GET ALL --------------------
    public async Task<List<PatientDto>> GetAllAsync()
    {
        var list = await _db.Patients.ToListAsync();
        return list.Select(MapToDto).ToList();
    }

    // -------------------- GET BY ID --------------------
    public async Task<PatientDto?> GetByIdAsync(long id)
    {
        var x = await _db.Patients.FindAsync(id);
        if (x == null) return null;

        return MapToDto(x);
    }

    // -------------------- CREATE --------------------
    public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
    {
        var entity = new Patient
        {
            Firstname = dto.FirstName,
            Middlename = dto.MiddleName,
            Lastname = dto.LastName,
            Gender = dto.Gender,
            Email = dto.Email,
            Phone = dto.Phone,
            Dateofbirth = ToDateOnly(dto.DateOfBirth),
            Address = dto.Address,
            Language = dto.Language,
            Maritalstatus = dto.MaritalStatus,
            Mrn = dto.MRN
        };

        _db.Patients.Add(entity);
        await _db.SaveChangesAsync();

        return MapToDto(entity);
    }

    // -------------------- UPDATE --------------------
    public async Task<PatientDto?> UpdateAsync(long id, UpdatePatientDto dto)
    {
        var entity = await _db.Patients.FindAsync(id);
        if (entity == null) return null;

        entity.Firstname = dto.FirstName;
        entity.Middlename = dto.MiddleName;
        entity.Lastname = dto.LastName;
        entity.Gender = dto.Gender;
        entity.Email = dto.Email;
        entity.Phone = dto.Phone;
        entity.Dateofbirth = ToDateOnly(dto.DateOfBirth);
        entity.Address = dto.Address;
        entity.Language = dto.Language;
        entity.Maritalstatus = dto.MaritalStatus;
        entity.Mrn = dto.MRN;

        await _db.SaveChangesAsync();

        return MapToDto(entity);
    }

    // -------------------- DELETE --------------------
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _db.Patients.FindAsync(id);
        if (entity == null) return false;

        _db.Patients.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}