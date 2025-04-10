namespace DataTranfer.Dtos;

public record SalaryCoefficientDto {
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}