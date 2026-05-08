using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class PatientAppointmentDashboard
{
    public long? Patientid { get; set; }

    public string? Patientname { get; set; }

    public string? Gender { get; set; }

    public string? Mrn { get; set; }

    public double? Age { get; set; }

    public long? Appointmentid { get; set; }

    public DateOnly? Appointmentdate { get; set; }

    public TimeOnly? Starttime { get; set; }

    public TimeOnly? Endtime { get; set; }

    public string? Visittype { get; set; }

    public string? Visitmode { get; set; }

    public string? Visitreason { get; set; }

    public string? Appointmentstatus { get; set; }

    public bool? Ispaid { get; set; }

    public long? Paymentid { get; set; }

    public long? Providerid { get; set; }

    public string? Providername { get; set; }

    public string? Speciality { get; set; }

    public int? Videosessionid { get; set; }

    public string? Videocallstatus { get; set; }

    public DateTime? Videostarttime { get; set; }

    public DateTime? Videoendtime { get; set; }

    public string? Recordingurl { get; set; }

    public string? Videoname { get; set; }

    public int? Durationseconds { get; set; }

    public bool? Canjoincall { get; set; }

    public bool? Canreschedule { get; set; }

    public bool? Cancancel { get; set; }

    public string? Meetinglink { get; set; }

    public string? Meetingid { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updateddate { get; set; }
}
