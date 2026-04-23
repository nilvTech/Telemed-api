// Models/Patientssummary.cs
namespace Telemed.Models;

public partial class Patientssummary
{
    public long Patientid { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Gender { get; set; }
    public DateOnly? Dateofbirth { get; set; }
    public string? Conditions { get; set; }
}