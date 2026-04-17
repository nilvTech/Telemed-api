// DTOs/UpdateSmartgoalDto.cs
namespace Telemed.DTOs;

public class UpdateSmartgoalDto
{
    public string? Goaltitle { get; set; }
    public string? Description { get; set; }
    public decimal? Targetvalue { get; set; }
    public decimal? Currentvalue { get; set; }
    public string? Unit { get; set; }
    public DateOnly? Targetdate { get; set; }
    public string? Status { get; set; }
    public string? Diettype { get; set; }
    public string? Exercisetype { get; set; }
    public int? Weeklyminutes { get; set; }
    public string? Notes { get; set; }
    public long? Updatedby { get; set; }
}