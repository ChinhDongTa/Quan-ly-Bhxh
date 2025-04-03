namespace DataTranfer.Dtos;

public record TokenResponseDto {
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
