namespace Dtos.Human;

public record RefreshTokenRequestDto {
    public Guid UserId { get; set; }
    public required string RefreshToken { get; set; }
}