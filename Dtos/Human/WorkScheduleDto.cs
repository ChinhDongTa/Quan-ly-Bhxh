namespace Dtos.Human;

public record WorkScheduleDto {
    public int Id { get; set; }
    public DateOnly StartDay { get; set; }
    public DateOnly EndDay { get; set; }
    public virtual ICollection<WorkDayDto> WorkDays { get; set; } = []; // Danh sách ngày làm việc trong lịch làm việc
    public string? UserId { get; set; } = null!; // UserId của người dùng tạo/cập nhập sau cùng lịch làm việc
    public string? InfoUserCreated { get; set; }
    public DateTime? UpdateAt { get; set; }  // Ngày giờ cập nhật gần nhất
}