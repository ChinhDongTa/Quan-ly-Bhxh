namespace Dtos.Human;

public record WorkDayDto {
    public int Id { get; set; }
    public DateOnly Date { get; set; }  // Ngày làm việc

    public ICollection<WorkShiftDto>? WorkShiftDtos { get; set; } // Danh sách lịch làm việc trong ngày
}