using DataServices.Entities.Human;
using Microsoft.EntityFrameworkCore;

namespace DataServices.Data;

public class BhxhDbContext(DbContextOptions<BhxhDbContext> options) : DbContext(options) {
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<QuarterDepartmentRank> QuarterDepartmentRanks { get; set; }

    public virtual DbSet<QuarterEmployeeRank> QuarterEmployeeRanks { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<SalaryCoefficient> SalaryCoefficients { get; set; }
}