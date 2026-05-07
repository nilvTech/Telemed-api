using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class PatientDashboard
{
    public long? Patientid { get; set; }

    public string? Patientname { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public string? Mrn { get; set; }

    public long? Appointmentid { get; set; }

    public DateOnly? Appointmentdate { get; set; }

    public TimeOnly? Appointmentstarttime { get; set; }

    public string? Appointmentstatus { get; set; }

    public string? Visittype { get; set; }

    public long? Providerid { get; set; }

    public string? Doctorname { get; set; }

    public string? Speciality { get; set; }

    public int? Videosessionid { get; set; }

    public string? Callstatus { get; set; }

    public DateTime? Starttime { get; set; }

    public DateTime? Endtime { get; set; }

    public string? Recordingurl { get; set; }

    public int? Systolic { get; set; }

    public int? Diastolic { get; set; }

    public int? Heartrate { get; set; }

    public int? Spo2 { get; set; }

    public int? Glucose { get; set; }

    public decimal? Temperature { get; set; }

    public decimal? Weight { get; set; }

    public DateTime? Readingdate { get; set; }

    public long? Claimid { get; set; }

    public string? Claimnumber { get; set; }

    public string? Payer { get; set; }

    public decimal? Totalamount { get; set; }

    public decimal? Paidamount { get; set; }

    public string? Billingstatus { get; set; }

    public int? Notificationid { get; set; }

    public string? Notificationmessage { get; set; }

    public DateTime? Notificationdate { get; set; }

    public int? Encounterid { get; set; }

    public DateTime? Encounterdate { get; set; }

    public string? Encounternotes { get; set; }
}
