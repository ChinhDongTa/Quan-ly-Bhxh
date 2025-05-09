namespace Dtos.Human;

public record WorkShiftDto {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Tên ca làm việc
    public string Description { get; set; } = string.Empty; // Mô tả ca làm việc

    public bool? IsEdit { get; set; } // Đã thay đổi ca làm việc hay chưa
}