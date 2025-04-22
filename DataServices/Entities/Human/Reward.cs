namespace DataServices.Entities.Human;

/// <summary>
/// Lớp đại diện cho phần thưởng.
/// </summary>
public partial class Reward {
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public string? Type { get; set; }

    public string? Classify { get; set; }

    public virtual ICollection<QuarterDepartmentRank>? QuarterDepartmentRanks { get; set; }

    public virtual ICollection<QuarterEmployeeRank>? QuarterEmployeeRanks { get; set; }
}