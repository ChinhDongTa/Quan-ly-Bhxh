namespace DataServices.Entities.Human;

public partial class QuarterEmployeeRank {
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int RewardId { get; set; }

    public byte Quarter { get; set; }

    public int Year { get; set; }

    public int? SelfScore { get; set; }

    public int? ResultScore { get; set; }

    public int? TotalWork { get; set; }

    public int? NumWorked { get; set; }

    public string? Note { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Reward Reward { get; set; } = null!;
}