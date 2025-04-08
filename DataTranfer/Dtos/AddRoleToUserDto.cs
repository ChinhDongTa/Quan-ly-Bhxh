namespace DataTranfer.Dtos;

public record AddRoleToUserDto {
    public string UserId { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
}