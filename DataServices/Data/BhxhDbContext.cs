using DataServices.Entities.Human;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataServices.Data;

//DbContextOptions<BhxhDbContext> options
public class BhxhDbContext : IdentityDbContext<ApiUser> {
    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EventLog> EventLogs { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<QuarterDepartmentRank> QuarterDepartmentRanks { get; set; }

    public virtual DbSet<QuarterEmployeeRank> QuarterEmployeeRanks { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }
    public virtual DbSet<Level> Levels { get; set; }
    public virtual DbSet<QuarterScoreDept> QuarterScoreDepts { get; set; }

    public virtual DbSet<SalaryCoefficient> SalaryCoefficients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    {
        optionsBuilder.UseSqlServer("Data Source=10.64.208.250;Initial Catalog=BhxhDb;User ID=sa;Password=2Peng@qu@y; Trust Server Certificate=True;");
    }
}