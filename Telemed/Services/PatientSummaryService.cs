using Microsoft.EntityFrameworkCore;
using Telemed.DTOs;
using Telemed.Mappers;
using Telemed.Models;
using Telemed.Services.Interfaces;

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
                         WHERE CAST(appointmentdate AS DATE) = CURRENT_DATE) AS ""PatientsToday"",

                        (SELECT COUNT(*)
                         FROM consultation 
                         WHERE status = 'Completed' 
                         AND CAST(createddate AS DATE) = CURRENT_DATE) AS ""ConsultationsCompleted"",

                        (SELECT COUNT(*)
                         FROM appointment 
                         WHERE status IN ('CheckedIn', 'Waiting') 
                         AND CAST(appointmentdate AS DATE) = CURRENT_DATE) AS ""PendingConsultations"",

                        (SELECT COUNT(*)
                         FROM appointment 
                         WHERE CAST(appointmentdate AS DATE) = CURRENT_DATE) AS ""TotalAppointments"",

                        (SELECT COUNT(*)
                         FROM patientalert 
                         WHERE severity = 'Critical' 
                         AND isacknowledged = FALSE) AS ""CriticalAlerts"",

                        (SELECT COUNT(*)
                         FROM patientfollowup 
                         WHERE CAST(followupdate AS DATE) = CURRENT_DATE) AS ""FollowupsDueToday""
                ";

                var result = await _context
                    .PatientSummaries
                    .FromSqlRaw(sql)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return result?.ToDto() ?? new PatientSummaryDto();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PatientSummary Error] {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");

                throw;
            }
        }
    }
}