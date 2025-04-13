using System.ComponentModel.DataAnnotations;

namespace DataServices.Entities.Human;

public partial class EventLog {
    public int Id { get; set; }
    [MaxLength(200)]
    public string? ActionName { get; set; }
    public string? Description { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    [MaxLength(100)]
    public string? Browser { get; set; }
    public string? UserId { get; set; }
    [MaxLength(20)]
    public string? IpAddress { get; set; }
    
    public virtual ApiUser? User { get; set; }
}