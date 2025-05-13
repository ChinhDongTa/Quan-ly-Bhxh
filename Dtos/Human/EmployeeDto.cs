namespace Dtos.Human;
/// <summary>
/// DTO đại diện cho thông tin nhân viên.
/// </summary>
public record EmployeeDto {
    /// <summary>
    /// Mã nhân viên
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Họ và tên lót
    /// </summary>
    public string FirstName { get; set; } = null!;
    /// <summary>
    /// Tên
    /// </summary>
    public string LastName { get; set; } = null!;
    /// <summary>
    /// Tên đầy đủ
    /// </summary>
    public string FullName { get => $"{FirstName} {LastName}"; }

    // Thông tin liên lạc
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }

    // Thông tin công việc
    public int? DeptId { get; set; }
    public string? DeptName { get; set; }
    public int? PostId { get; set; }
    public string? PositionName { get; set; }

    // Thông tin tài chính
    public int? SalaryCoefficientId { get; set; }
    public string? Salary { get; set; }
    public string? AccountBank { get; set; }

    // Thông tin cá nhân
    public DateTime? Birthdate { get; set; }
    public bool Gender { get; set; }
    public string? IdentityCard { get; set; }

    // Trạng thái
    /// <summary>
    /// Trạng thái nghỉ việc của nhân viên (true: Đã nghỉ, false: Đang làm việc).
    /// </summary>
    public bool IsQuitJob { get; set; }
    /// <summary>
    /// Thứ tự sắp xếp của nhân viên.
    /// </summary>
    public int? SortOrder { get; set; }
    /// <summary>
    /// ID Telegram của nhân viên.
    /// </summary>
    public long? TelegramId { get; set; }
}