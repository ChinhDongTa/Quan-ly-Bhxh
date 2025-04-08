namespace DataTranfer.Dtos;

public record InfoDto {
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}