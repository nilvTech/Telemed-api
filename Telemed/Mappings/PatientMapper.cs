using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappings;

public static class PatientMapper
{
    // Converters
    private static DateTime? ToDateTime(DateOnly? d)
    {
        return d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : null;
    }

    private static DateOnly? ToDateOnly(DateTime? d)
    {
        return d.HasValue ? DateOnly.FromDateTime(d.Value) : null;
    }

    // Entity -> DTO
    public static PatientDto ToDto(this Patient p)
    {
        return new PatientDto
        {
            PatientId = p.Patientid,
            FirstName = p.Firstname,
            MiddleName = p.Middlename,
            LastName = p.Lastname,
            Gender = p.Gender,
            DateOfBirth = ToDateTime(p.Dateofbirth),
            Email = p.Email,
            Phone = p.Phone
        };
    }

    // DTO -> Entity
    public static Patient ToEntity(this CreatePatientDto dto)
    {
        return new Patient
        {
            Firstname = dto.FirstName,
            Middlename = dto.MiddleName,
            Lastname = dto.LastName,
            Gender = dto.Gender,
            Dateofbirth = ToDateOnly(dto.DateOfBirth),
            Email = dto.Email,
            Phone = dto.Phone
        };
    }
}