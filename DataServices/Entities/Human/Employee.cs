﻿namespace DataServices.Entities.Human;

public partial class Employee {
    public int EmployeeId { get; set; }

    public string? Email { get; set; }

    public string? IdentityCard { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? DeptId { get; set; }

    public int? PostId { get; set; }

    public int? SalaryCoefficientId { get; set; }

    public string? AccountBank { get; set; }

    public DateTime? Birthdate { get; set; }

    public bool IsQuitJob { get; set; }

    public int? SortOrder { get; set; }
    public long? TelegramId { get; set; }
    public bool Gender { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual Position? Post { get; set; }

    public virtual ICollection<QuarterEmployeeRank>? QuarterEmployeeRanks { get; set; } = [];
    public virtual ICollection<ApiUser>? ApiUsers { get; set; } = [];

    public virtual SalaryCoefficient? SalaryCoefficient { get; set; }
}