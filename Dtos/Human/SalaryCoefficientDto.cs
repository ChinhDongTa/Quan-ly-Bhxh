namespace Dtos.Human;

public record SalaryCoefficientDto {
    public required int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}