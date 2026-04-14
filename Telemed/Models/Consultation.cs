using System;
using System.Collections.Generic;

namespace Telemed.Models;

public partial class Consultation
{
    public long Consultationid { get; set; }

    public long Appointmentid { get; set; }

    public long Patientid { get; set; }

    public long Providerid { get; set; }

    public DateTime? Starttime { get; set; }

    public DateTime? Endtime { get; set; }

    public int? Durationminutes { get; set; }

    public string? Chiefcomplaint { get; set; }

    public string? Diagnosis { get; set; }

    public string? Treatmentplan { get; set; }

    public string? Notes { get; set; }

    public decimal? Temperature { get; set; }

    public string? Bloodpressure { get; set; }

    public int? Pulse { get; set; }

    public int? Respiratoryrate { get; set; }

    public int? Oxygensaturation { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly? Followupdate { get; set; }

    public string? Followupnotes { get; set; }

    public string? Recordingurl { get; set; }

    public bool Isprescriptiongenerated { get; set; }

    public long? Createdby { get; set; }

    public DateTime Createddate { get; set; }

    public long? Updatedby { get; set; }

    public DateTime? Updateddate { get; set; }

    public bool Isactive { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual ICollection<Consultationdiagnosis> Consultationdiagnoses { get; set; } = new List<Consultationdiagnosis>();

    public virtual ICollection<Consultationnote> Consultationnotes { get; set; } = new List<Consultationnote>();

    public virtual ICollection<Consultationprescription> Consultationprescriptions { get; set; } = new List<Consultationprescription>();

    //patient condition

    public virtual ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();



    // condition



    public virtual Patient Patient { get; set; } = null!;

    public virtual Provider Provider { get; set; } = null!;

   // public virtual PatientCondition? PatientCondition { get; set; }
}
