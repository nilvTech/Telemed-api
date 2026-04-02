// Telemed/Mappers/PatientSummaryMapper.cs
using Telemed.DTOs;

namespace Telemed.Mappers
{
    public static class PatientSummaryMapper
    {
        /// <summary>
        /// Maps the result from FromSqlRaw to DTO with null safety
        /// </summary>
        public static PatientSummaryDto ToDto(this PatientSummaryDto? source)
        {
            if (source == null)
            {
                return new PatientSummaryDto();
            }

            return new PatientSummaryDto
            {
                PatientsToday = source.PatientsToday,
                ConsultationsCompleted = source.ConsultationsCompleted,
                PendingConsultations = source.PendingConsultations,
                TotalAppointments = source.TotalAppointments,
                CriticalAlerts = source.CriticalAlerts,
                FollowupsDueToday = source.FollowupsDueToday
            };
        }

        /// <summary>
        /// Creates DTO directly from individual counts (useful if you switch to pure EF Core LINQ later)
        /// </summary>
        public static PatientSummaryDto ToDto(
            int patientsToday,
            int consultationsCompleted,
            int pendingConsultations,
            int totalAppointments,
            int criticalAlerts,
            int followupsDueToday)
        {
            return new PatientSummaryDto
            {
                PatientsToday = patientsToday,
                ConsultationsCompleted = consultationsCompleted,
                PendingConsultations = pendingConsultations,
                TotalAppointments = totalAppointments,
                CriticalAlerts = criticalAlerts,
                FollowupsDueToday = followupsDueToday
            };
        }
    }
}