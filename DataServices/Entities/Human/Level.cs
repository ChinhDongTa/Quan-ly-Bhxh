using System.ComponentModel.DataAnnotations;

namespace DataServices.Entities.Human;

/// <summary>
/// Cấp hành chính
/// </summary>
public partial class Level {
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? ShortName { get; set; }
    public virtual ICollection<Department>? Departments { get; set; }
}