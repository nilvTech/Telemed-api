// DTOs/ProviderDashboardStatsDto.cs
namespace Telemed.DTOs;

public class ProviderDashboardStatsDto
{
    public int PatientsToday { get; set; }
    public int AppointmentsCompleted { get; set; }
    public int AppointmentsPending { get; set; }
    public int TotalAppointments { get; set; }
}