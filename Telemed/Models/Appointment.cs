using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Appointment
{
    public long Appointmentid { get; set; }

    public long Patientid { get; set; }

    public long Providerid { get; set; }

    public long? Clinicid { get; set; }

    public DateOnly Appointmentdate { get; set; }

    public TimeOnly Starttime { get; set; }

    public TimeOnly? Endtime { get; set; }

    public string Visittype { get; set; } = null!;

    public string Visitmode { get; set; } = null!;

    public string? Visitreason { get; set; }

    public string Status { get; set; } = null!;

    public int? Tokennumber { get; set; }

    public int? Queueposition { get; set; }

    public DateTime? Checkedintime { get; set; }

    public int? Waitingminutes { get; set; }

    public string? Meetinglink { get; set; }

    public string? Meetingid { get; set; }

    public bool Ispaid { get; set; }

    public long? Paymentid { get; set; }

    public bool Isfollowup { get; set; }

    public long? Parentappointmentid { get; set; }

    public string? Priority { get; set; }

    public long? Createdby { get; set; }

    public DateTime Createddate { get; set; }

    public long? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public bool Isactive { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual Patient Patient { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;
}
