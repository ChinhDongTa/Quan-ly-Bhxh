namespace DataServices.Entities.Human;

public class User {
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int EmployeeId { get; set; }
    public int RoleId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public virtual Role? Role { get; set; }
}