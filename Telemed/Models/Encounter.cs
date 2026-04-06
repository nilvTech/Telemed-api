using Telemed.Models;

public partial class Encounter
{
    public long Encounterid { get; set; }
    public long Appointmentid { get; set; }
    public long Patientid { get; set; }
    public long Providerid { get; set; }
    public DateTime Encounterdate { get; set; }
    public string? Subjective { get; set; }
    public string? Objective { get; set; }
    public string? Assessment { get; set; }
    public string? Plan { get; set; }
    public string? Diagnosis { get; set; }
    public string? Icd10code { get; set; }
    public string? Notes { get; set; }
    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }

    public virtual Patient Patient { get; set; } = null!;
    public virtual Provider Provider { get; set; } = null!;
    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public virtual ICollection<Videosession> Videosessions { get; set; } = new List<Videosession>();
    public virtual ICollection<Vital> Vitals { get; set; } = new List<Vital>();

    // ✅ Add this
    public virtual Appointment Appointment { get; set; } = null!;
}