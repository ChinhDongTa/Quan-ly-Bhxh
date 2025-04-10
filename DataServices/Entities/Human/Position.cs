namespace DataServices.Entities.Human;

public partial class Position {
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}