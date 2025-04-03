namespace DataServices.Entities.Human;

/// <summary>
/// Cấp hành chính
/// </summary>
public partial class Level {
    public int LevelId { get; set; }
    public string Name { get; set; } = null!;
    public string? ShortName { get; set; }
    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}