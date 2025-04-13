namespace DataServices.Entities.Human;

public partial class SalaryCoefficient {
    public int Id { get; set; }

    public byte Rank { get; set; }

    public double Coeficient { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Employee>? Employees { get; set; }
}