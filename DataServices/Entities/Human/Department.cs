﻿namespace DataServices.Entities.Human;

public partial class Department {
    public int Id { get; set; }

    public string ShortName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? Score { get; set; }

    public bool IsActive { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? SortOrder { get; set; }

    public int LevelId { get; set; }
    public virtual Level? Level { get; set; }
    public virtual ICollection<Employee>? Employees { get; set; }

    public virtual ICollection<QuarterDepartmentRank>? QuarterDepartmentRanks { get; set; }
    public virtual ICollection<QuarterScoreDept>? QuarterScoreDepts { get; set; }
}