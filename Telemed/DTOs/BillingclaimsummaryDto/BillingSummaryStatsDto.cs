// DTOs/BillingSummaryStatsDto.cs
namespace Telemed.DTOs;

public class BillingSummaryStatsDto
{
    public int TotalClaims { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal PendingAmount { get; set; }
    public decimal DeniedAmount { get; set; }
    public Dictionary<string, int> ClaimsByStatus { get; set; }
        = new Dictionary<string, int>();
    public Dictionary<string, decimal> AmountByStatus { get; set; }
        = new Dictionary<string, decimal>();
}