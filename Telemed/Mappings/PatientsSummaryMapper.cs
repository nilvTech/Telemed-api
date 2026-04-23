// Mappers/PatientSummaryMapper.cs
using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class PatientsSummaryMapper
{
    private static int? CalculateAge(DateOnly? dob)
    {
        if (!dob.HasValue) return null;

        var today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - dob.Value.Year;

        if (dob.Value > today.AddYears(-age))
            age--;

        return age;
    }

    public static PatientsSummaryResponseDto ToResponseDto(
        Patientssummary entity)
    {
        // Split conditions string into list
        var conditionList = !string.IsNullOrEmpty(entity.Conditions)
            ? entity.Conditions
                .Split(',', StringSplitOptions.TrimEntries |
                            StringSplitOptions.RemoveEmptyEntries)
                .ToList()
            : new List<string>();

        return new PatientsSummaryResponseDto
        {
            Patientid = entity.Patientid,
            Firstname = entity.Firstname,
            Lastname = entity.Lastname,
            Fullname = $"{entity.Firstname} {entity.Lastname}".Trim(),
            Gender = entity.Gender,
            Dateofbirth = entity.Dateofbirth,
            Age = CalculateAge(entity.Dateofbirth),
            Conditions = entity.Conditions,
            Conditioncount = conditionList.Count,
            Conditionlist = conditionList
        };
    }
}