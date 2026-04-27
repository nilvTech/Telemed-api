// Mappers/CareteampatientMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class CareteampatientMapper
{
    private static DateTime ToUnspecified(DateTime dt)
        => DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

    private static int? CalculateAge(DateOnly? dob)
    {
        if (!dob.HasValue) return null;
        var today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - dob.Value.Year;
        if (dob.Value > today.AddYears(-age)) age--;
        return age;
    }

    public static Careteampatient ToEntity(CreateCareteampatientDto dto)
    {
        return new Careteampatient
        {
            Patientid = dto.Patientid,
            Careteamid = dto.Careteamid,
            Assigneddate = ToUnspecified(DateTime.UtcNow),
            Isactive = true
        };
    }

    public static void UpdateEntity(
        Careteampatient entity, UpdateCareteampatientDto dto)
    {
        if (dto.Isactive.HasValue)
            entity.Isactive = dto.Isactive;
    }

    public static CareteampatientResponseDto ToResponseDto(
        Careteampatient entity)
    {
        return new CareteampatientResponseDto
        {
            Careteampatientid = entity.Careteampatientid,

            // Patient
            Patientid = entity.Patientid,
            Patientname = entity.Patient != null
                                ? $"{entity.Patient.Firstname} {entity.Patient.Middlename} {entity.Patient.Lastname}"
                                  .Replace("  ", " ").Trim()
                                : null,
            Mrn = entity.Patient?.Mrn,
            Gender = entity.Patient?.Gender,
            Dateofbirth = entity.Patient?.Dateofbirth,
            Age = CalculateAge(entity.Patient?.Dateofbirth),

            // Careteam
            Careteamid = entity.Careteamid,
            Teamname = entity.Careteam?.Teamname,
            Teamdescription = entity.Careteam?.Description,

            // Assignment
            Assigneddate = entity.Assigneddate,
            Isactive = entity.Isactive,
            StatusLabel = entity.Isactive == true ? "Active" : "Inactive"
        };
    }
}