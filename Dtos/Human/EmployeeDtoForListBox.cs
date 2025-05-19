namespace Dtos.Human;

/// <summary>
/// Dùng cho ListBox
/// </summary>
/// <param name="Id"></param>
/// <param name="FullName"></param>
public record EmployeeDtoForListBox(
  /// <summary>
  /// Mã nhân viên staffId
  /// </summary>
  int Id = default,

     /// <summary>
     /// Họ tên nhân viên + đơn vị làm việc
     /// </summary>
     string FullName = ""
);

public record EmployeeDtoGroupByDept {
    public int DeptId { get; set; }
    public required string Name { get; set; }
    public required IEnumerable<EmployeeSimpleDto> EmployeeSimpleDtos { get; set; }
}
public record EmployeeSimpleDto(int Id, string FullName);