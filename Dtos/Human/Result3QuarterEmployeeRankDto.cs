namespace Dtos.Human;

/// <summary>
/// Kết quả xếp loại của 3 quý trước
/// </summary>
public record Result3QuarterEmployeeRankDto {
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string TotalReward { get; set; } = string.Empty;
}