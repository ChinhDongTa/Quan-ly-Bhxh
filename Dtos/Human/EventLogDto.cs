namespace Dtos.Human;

public record EventLogDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? ActionName { get; set; }
    public string? Description { get; set; }
    public DateTime CreateTime { get; set; } = DateTime.Now;
    public string? Browser { get; set; }
    public string? IpAddress { get; set; }
    public string? EmployeeName { get; set; }
}