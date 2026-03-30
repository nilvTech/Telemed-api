using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Encounter
{
    public int Encounterid { get; set; }

    public int Appointmentid { get; set; }

    public int Patientid { get; set; }

    public int Providerid { get; set; }

    public DateTime Encounterdate { get; set; }

    public string? Subjective { get; set; }

    public string? Objective { get; set; }

    public string? Assessment { get; set; }

    public string? Plan { get; set; }

    public string? Diagnosis { get; set; }

    public string? Icd10code { get; set; }

    public string? Notes { get; set; }

    public DateTime? Createdate { get; set; }

    public DateTime? Updatedate { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual Provider Provider { get; set; } = null!;

    public virtual ICollection<Videosession> Videosessions { get; set; } = new List<Videosession>();

    public virtual ICollection<Vital> Vitals { get; set; } = new List<Vital>();
}
