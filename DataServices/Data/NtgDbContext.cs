using DataServices.Entities.Tst;
using Microsoft.EntityFrameworkCore;

namespace DataServices.Data;

public class NtgDbContext : DbContext {
    public virtual DbSet<Ntg> Ntgs { get; set; } = null!;
    public virtual DbSet<CoQuanBhxh> CoQuanBhxhs { get; set; } = null!;
    public virtual DbSet<ImportHistory> ImportHistories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=10.64.208.250;Initial Catalog=NtgDb;User ID=sa;Password=2Peng@qu@y; Trust Server Certificate=True;");
    }
}