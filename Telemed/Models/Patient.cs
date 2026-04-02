using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Patient
{
    public long Patientid { get; set; }

    public string? Firstname { get; set; }

    public string? Middlename { get; set; }

    public string? Lastname { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public string? Ssn { get; set; }

    public string? Mrn { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Language { get; set; }

    public string? Maritalstatus { get; set; }

    public string? Timezone { get; set; }

    public string? Preferredcommunicationchannel { get; set; }

    public string? Soapnotes { get; set; }

    public string? Careteam { get; set; }

    public string? Primaryprovider { get; set; }

    public string? Insurancetype { get; set; }

    public string? Holdername { get; set; }

    public string? Relationtoinsured { get; set; }

    public string? Companyname { get; set; }

    public string? Policynumber { get; set; }

    public string? Employername { get; set; }

    public string? Groupname { get; set; }

    public string? Plan { get; set; }

    public DateOnly? Effectivedate { get; set; }

    public bool? Insuranceverified { get; set; }

    public bool? Insurancevalid { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Patientalert> Patientalerts { get; set; } = new List<Patientalert>();

    public virtual ICollection<Patientfollowup> Patientfollowups { get; set; } = new List<Patientfollowup>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<Videosession> Videosessions { get; set; } = new List<Videosession>();

    public virtual ICollection<Vital> Vitals { get; set; } = new List<Vital>();
}
