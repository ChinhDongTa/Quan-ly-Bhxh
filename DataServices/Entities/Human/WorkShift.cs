namespace DataServices.Entities.Human;

/// <summary>
/// Lớp đại diện cho ca làm việc.
/// </summary>
public partial class WorkShift {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Tên ca làm việc
    public string Description { get; set; } = string.Empty; // Mô tả ca làm việc
    public int WorkDayId { get; set; }
    public virtual WorkDay? WorkDay { get; set; } // Ngày làm việc
}