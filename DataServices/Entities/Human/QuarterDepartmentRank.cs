namespace DataServices.Entities.Human;

public partial class QuarterDepartmentRank {
    public int Id { get; set; }

    public int DeptId { get; set; }

    public int RewardId { get; set; }

    public byte Quarter { get; set; }

    public int Year { get; set; }

    public int? SelfScore { get; set; }

    public int? ResultScore { get; set; }

    public string? Note { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual Reward? Reward { get; set; }
    public virtual ICollection<QuarterScoreDept>? QuarterScoreDepts { get; set; }
}