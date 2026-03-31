using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Provider
{
    public long Providerid { get; set; }

    public string Providername { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Speciality { get; set; }

    public string? Website { get; set; }

    public string? Primaryaddress { get; set; }

    public string? Status { get; set; }

    public string? Action { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<Videosession> Videosessions { get; set; } = new List<Videosession>();
}
