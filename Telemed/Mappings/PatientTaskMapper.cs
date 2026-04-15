using Telemed.DTOs;
using Telemed.Models;

namespace Telemed.Mappers;

public static class PatientTaskMapper
{
    public static PatientTask ToEntity(CreatePatientTaskDto dto)
    {
        return new PatientTask
        {
            Taskname = dto.Taskname,
            Duedate = dto.Duedate,
            Patientid = dto.Patientid,
            Providerinfoid = dto.Providerinfoid,
            Status = dto.Status ?? "Pending",
            Priority = dto.Priority ?? "Medium",
            Description = dto.Description,
            Createdby = dto.Createdby,
            Createdat = DateTime.UtcNow,
            Updatedat = DateTime.UtcNow
        };
    }

    public static TaskResponseDto ToResponseDto(PatientTask t)
    {
        return new TaskResponseDto
        {
            Taskid = t.Taskid,
            Taskname = t.Taskname,
            Duedate = t.Duedate,

            Patientid = t.Patientid,
            Patientname = t.Patient != null
                ? $"{t.Patient.Firstname} {t.Patient.Lastname}"
                : null,

            Providerinfoid = t.Providerinfoid,
            Providername = t.Providerinfo != null
                ? $"{t.Providerinfo.Firstname} {t.Providerinfo.Lastname}"
                : null,

            Status = t.Status,
            Priority = t.Priority,
            Description = t.Description,
            Completeddate = t.Completeddate,
            Createdat = t.Createdat,
            Updatedat = t.Updatedat
        };
    }
}