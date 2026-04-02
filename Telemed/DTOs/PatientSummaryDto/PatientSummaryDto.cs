// Telemed.DTOs/PatientSummaryDto.cs
namespace Telemed.DTOs
{
    public class PatientSummaryDto
    {
        public int PatientsToday { get; set; }
        public int ConsultationsCompleted { get; set; }
        public int PendingConsultations { get; set; }
        public int TotalAppointments { get; set; }
        public int CriticalAlerts { get; set; }
        public int FollowupsDueToday { get; set; }
    }
}