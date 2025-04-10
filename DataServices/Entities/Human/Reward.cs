namespace DataServices.Entities.Human;

public partial class Reward {
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public string? Type { get; set; }

    public string? Classify { get; set; }

    public virtual ICollection<QuarterDepartmentRank> QuarterDepartmentRanks { get; set; } = new List<QuarterDepartmentRank>();

    public virtual ICollection<QuarterEmployeeRank> QuarterEmployeeRanks { get; set; } = new List<QuarterEmployeeRank>();
}