using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;
using System;

namespace Telemed.Services
{
    public class PatientSummaryService : IPatientSummaryService
    {
        private readonly TelemedDbContext _context;

        public PatientSummaryService(TelemedDbContext context)
        {
            _context = context;
        }

        public async Task<PatientSummaryDto> GetPatientSummaryAsync()
        {
            try
            {
                var sql = @"
                    SELECT
                        (SELECT COUNT(DISTINCT patientid)
                         FROM appointment 
                         WHERE DATE(appointmentdate) = CURRENT_DATE) AS ""PatientsToday"",

                        -- Changed logic: Count as Completed if status is 'Completed' OR 'onHold'
                        (SELECT COUNT(*)
                         FROM consultation 
                         WHERE status IN ('Completed', 'onHold')
                         AND DATE(createddate) = CURRENT_DATE) AS ""ConsultationsCompleted"",

                        (SELECT COUNT(*)
                         FROM appointment 
                         WHERE status IN ('CheckedIn', 'Waiting', 'onHold', 'Scheduled', 'InProgress')
                         AND DATE(appointmentdate) = CURRENT_DATE) AS ""PendingConsultations"",

                        (SELECT COUNT(*)
                         FROM appointment 
                         WHERE DATE(appointmentdate) = CURRENT_DATE) AS ""TotalAppointments"",

                        (SELECT COUNT(*)
                         FROM patientalert 
                         WHERE severity = 'Critical' 
                         AND isacknowledged = FALSE) AS ""CriticalAlerts"",

                        (SELECT COUNT(*)
                         FROM patientfollowup 
                         WHERE DATE(followupdate) = CURRENT_DATE) AS ""FollowupsDueToday""
                ";

                var result = await _context.PatientSummaries
                    .FromSqlRaw(sql)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                var dto = result?.ToDto() ?? new PatientSummaryDto();

                // Logging
                Console.WriteLine($"[PatientSummary] PatientsToday: {dto.PatientsToday} | " +
                                  $"TotalAppts: {dto.TotalAppointments} | " +
                                  $"Pending: {dto.PendingConsultations} | " +
                                  $"Completed: {dto.ConsultationsCompleted} | " +
                                  $"CriticalAlerts: {dto.CriticalAlerts} | " +
                                  $"FollowupsToday: {dto.FollowupsDueToday}");

                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PatientSummaryService ERROR] {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");

                return new PatientSummaryDto();
            }
        }
    }
}