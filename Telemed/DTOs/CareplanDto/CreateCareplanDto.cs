// DTOs/CreateCareplanDto.cs
namespace Telemed.DTOs;

public class CreateCareplanDto
{
    public long Patientid { get; set; }
    public long? Providerinfoid { get; set; }

    // Basic
    public DateOnly Startdate { get; set; }
    public DateOnly? Enddate { get; set; }
    public string? Status { get; set; }
    public string? Risklevel { get; set; }
    public int? Ccmminutes { get; set; }

    // Blood Pressure Targets
    public int? Bpsystolictarget { get; set; }
    public int? Bpdiastolictarget { get; set; }

    // Diabetes Targets
    public decimal? Hba1ctarget { get; set; }
    public int? Glucosetargetmin { get; set; }
    public int? Glucosetargetmax { get; set; }

    // Vitals Targets
    public int? Spo2target { get; set; }
    public int? Heartratetargetmin { get; set; }
    public int? Heartratetargetmax { get; set; }

    // Weight Targets
    public decimal? Weighttarget { get; set; }
    public decimal? Bmitarget { get; set; }

    // Lipid Target
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

    public long? Createdby { get; set; }
}