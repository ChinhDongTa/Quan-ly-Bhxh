namespace DataServices.Entities.Human;

/// <summary>
/// Lớp đại diện cho lịch làm việc.
/// </summary>
public partial class WorkSchedule {
    public int Id { get; set; }

    /// <summary>
    /// Ngày bắt đầu của lịch làm việc là thứ hai
    /// </summary>
    public DateOnly StartDay { get; set; }

    /// <summary>
    /// Ngày kết thúc của lịch làm việc là chủ nhật
    ///</summary>
    public DateOnly EndDay { get; set; }

    public virtual ICollection<WorkDay> WorkDays { get; set; } = []; // Danh sách ngày làm việc trong lịch làm việc
    public string? UserId { get; set; } = null!; // UserId của người dùng tạo/cập nhập sau cùng lịch làm việc
    public virtual ApiUser? User { get; set; } //
    public DateTime? UpdateAt { get; set; } = DateTime.Now; // Ngày giờ cập nhật gần nhất
}