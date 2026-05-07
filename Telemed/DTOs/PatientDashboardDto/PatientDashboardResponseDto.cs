// DTOs/PatientDashboardResponseDto.cs
namespace Telemed.DTOs;

public class PatientDashboardResponseDto
{
    // ===== Patient Info =====
    public long? Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dateofbirth { get; set; }
    public int? Age { get; set; }               // Auto calculated
    public string? Mrn { get; set; }

    // ===== Upcoming Appointment =====
    public long? Appointmentid { get; set; }
    public DateOnly? Appointmentdate { get; set; }
    public TimeOnly? Appointmentstarttime { get; set; }
    public string? Appointmentstatus { get; set; }
    public string? Visittype { get; set; }
    public bool HasAppointment { get; set; }
    public bool IsAppointmentToday { get; set; }

    // ===== Provider =====
    public long? Providerid { get; set; }
    public string? Doctorname { get; set; }
    public string? Speciality { get; set; }

    // ===== Video Session =====
    public int? Videosessionid { get; set; }
    public string? Callstatus { get; set; }
    public DateTime? Starttime { get; set; }
    public DateTime? Endtime { get; set; }
    public string? Recordingurl { get; set; }
    public bool HasVideoSession { get; set; }

    // ===== Health Summary (Latest RPM) =====
    public int? Systolic { get; set; }
    public int? Diastolic { get; set; }
    public string? Bloodpressure { get; set; }      // Auto "120/80"
    public string? BpStatus { get; set; }           // Normal / High
    public int? Heartrate { get; set; }
    public string? HeartrateStatus { get; set; }
    public int? Spo2 { get; set; }
    public string? Spo2Status { get; set; }
    public int? Glucose { get; set; }
    public decimal? Temperature { get; set; }
    public decimal? Weight { get; set; }
    public DateTime? Readingdate { get; set; }
    public bool HasRpmData { get; set; }

    // ===== Billing Summary =====
    public long? Claimid { get; set; }
    public string? Claimnumber { get; set; }
    public string? Payer { get; set; }
    public decimal? Totalamount { get; set; }
    public decimal? Paidamount { get; set; }
    public decimal? Balancedue { get; set; }        // Auto calculated
    public string? Billingstatus { get; set; }
    public bool HasClaim { get; set; }

    // ===== Notifications =====
    public int? Notificationid { get; set; }
    public string? Notificationmessage { get; set; }
    public DateTime? Notificationdate { get; set; }
    public bool HasNotification { get; set; }

    // ===== Recent Visit =====
    public int? Encounterid { get; set; }
    public DateTime? Encounterdate { get; set; }
    public string? Encounternotes { get; set; }
    public bool HasEncounter { get; set; }
}