using DongTa.TypeExtension;

namespace ApiGateway.Controllers.File.Models;
public record TongHopQuyCcVc {
    public byte STT { get; set; }
    public required string StaffName { get; set; }
    public int TotalWork { get; set; }
    public int NumWorked { get; set; }
    public string PercentWork => TotalWork != 0 ? ((double)NumWorked / TotalWork).ToPercentString(2) : "0";
    public byte WorkNotDone => (byte)(TotalWork - NumWorked);
    public string Reward { get; set; } = string.Empty;
    public string SelfReward { get; set; } = string.Empty;
    public string? Note { get; set; }
}