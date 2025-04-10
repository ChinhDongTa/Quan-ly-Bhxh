namespace DataTranfer.Dtos;

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