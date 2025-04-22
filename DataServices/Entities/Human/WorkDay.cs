namespace DataServices.Entities.Human;

/// <summary>
/// Lớp đại diện cho ngày làm việc.
/// </summary>
public partial class WorkDay {
    public int Id { get; set; }
    public DateOnly Date { get; set; } // Ngày làm việc
    public int WorkScheduleId { get; set; } // ID của lịch làm việc
    public virtual WorkSchedule? WorkSchedule { get; set; } // Lịch làm việc trong ngày
    public virtual ICollection<WorkShift>? WorkShifts { get; set; } // Danh sách lịch làm việc trong ngày
}