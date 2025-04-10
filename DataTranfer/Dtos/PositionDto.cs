namespace DataTranfer.Dtos;

public record PositionDto {
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public string? ShortName { get; set; }
}