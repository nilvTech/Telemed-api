// DTOs/UpdateCareplanDto.cs
namespace Telemed.DTOs;

public class UpdateCareplanDto
{
    public long? Providerinfoid { get; set; }
    public DateOnly? Enddate { get; set; }
    public string? Status { get; set; }
    public string? Risklevel { get; set; }
    public int? Ccmminutes { get; set; }

    public int? Bpsystolictarget { get; set; }
    public int? Bpdiastolictarget { get; set; }
    public decimal? Hba1ctarget { get; set; }
    public int? Glucosetargetmin { get; set; }
    public int? Glucosetargetmax { get; set; }
    public int? Spo2target { get; set; }
    public int? Heartratetargetmin { get; set; }
    public int? Heartratetargetmax { get; set; }
    public decimal? Weighttarget { get; set; }
    public decimal? Bmitarget { get; set; }
    public decimal? Ldltarget { get; set; }

    public string? Problems { get; set; }
    public string? Goals { get; set; }
    public string? Interventions { get; set; }
    public string? Medications { get; set; }
    public string? Allergies { get; set; }

    public DateOnly? Nextreviewdate { get; set; }
    public DateOnly? Lastreviewdate { get; set; }
    public string? Reviewfrequency { get; set; }

    public long? Updatedby { get; set; }
}