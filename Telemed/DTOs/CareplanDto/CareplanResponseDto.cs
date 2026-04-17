// DTOs/CareplanResponseDto.cs
namespace Telemed.DTOs;

public class CareplanResponseDto
{
    public long Careplanid { get; set; }

    // Patient Info
    public long Patientid { get; set; }
    public string? Patientname { get; set; }
    public string? Mrn { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dateofbirth { get; set; }

    // Provider Info
    public long? Providerinfoid { get; set; }
    public string? Providername { get; set; }
    public string? Providerspeciality { get; set; }

    // Plan Dates
    public DateOnly Startdate { get; set; }
    public DateOnly? Enddate { get; set; }
    public string? Status { get; set; }
    public string? Risklevel { get; set; }
    public int? Ccmminutes { get; set; }
    public string? CcmStatus { get; set; }      // Met / NotMet (20min threshold)

    // Blood Pressure Targets
    public int? Bpsystolictarget { get; set; }
    public int? Bpdiastolictarget { get; set; }
    public string? BptargetLabel { get; set; }  // e.g. "<130/80"

    // Diabetes Targets
    public decimal? Hba1ctarget { get; set; }
    public int? Glucosetargetmin { get; set; }
    public int? Glucosetargetmax { get; set; }
    public string? GlucosetargetLabel { get; set; } // e.g. "80-130 mg/dL"

    // Vitals Targets
    public int? Spo2target { get; set; }
    public int? Heartratetargetmin { get; set; }
    public int? Heartratetargetmax { get; set; }
    public string? HeartratetargetLabel { get; set; } // e.g. "60-100 bpm"

    // Weight Targets
    public decimal? Weighttarget { get; set; }
    public decimal? Bmitarget { get; set; }

    // Lipid
    public decimal? Ldltarget { get; set; }

    // Clinical Details
    public string? Problems { get; set; }
    public string? Goals { get; set; }
    public string? Interventions { get; set; }
    public string? Medications { get; set; }
    public string? Allergies { get; set; }

    // Review Schedule
    public DateOnly? Nextreviewdate { get; set; }
    public DateOnly? Lastreviewdate { get; set; }
    public string? Reviewfrequency { get; set; }
    public bool IsOverdueForReview { get; set; }
    public int? DaysUntilNextReview { get; set; }

    public DateTime? Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}