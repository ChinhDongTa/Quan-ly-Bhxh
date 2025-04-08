namespace DefaultValue;
public record InfoMessage {
    public static string PostFailed => "Error: Xảy ra lổi cập nhật dữ liệu";
    public static string DownloadFailed => "Error: Download failed !";
    public static string Unknow => "Error: Không xác định được!";
    public static string DownloadSuccess => "Tải file thành công !";
    public static string Success => "Success: Đã xử lí thành công";
    public static string Unselect(string name) => $"Error: Chưa chọn {name}";
    public static string ApiCallFailed(string name) => $"Error: Lỗi khi lấy đối tượng '{name}'";
    public static string ObjectNull(string objName) => $"Error, {objName} is null";
    public static string NotAuthorized => "Warning: tài khoản của bạn không thể vào chức năng này!";
    public static string NotFound => "Warning, không tìm thấy kết quả";
    public static string NotSupport(string objectName) => $"Warning, {objectName} không được hỗ trợ !";
    public static string DateTimeInvalid => "Warning: ngày tháng năm không hợp lệ";
    public static string InputEmpty => "Error: Không có dữ liệu nhập vào !";
    public static string InvalidId(string name) => $"Error: Mã {name} không hợp lệ";
    public static string ActionSuccess(CRUD actionName, string objName) => $"Success: {actionName.CRUD2String()} {objName} thành công";
    public static string ActionFailed(CRUD actionName, string objName) => $"Error: {actionName.CRUD2String()} {objName} thất bại";
    public static string CrudResult(CRUD actionName, bool? result, string objName) => (result ?? false) ? ActionSuccess(actionName, objName) : ActionFailed(actionName, objName);
    public static string ActionSuccess(string actionName) => $"Success: {actionName} thành công !";
    public static string ActionFailed(string actionName) => $"Error: {actionName} thất bại !";
    public static string EmailOrPasswordInvalid => "Error: Email hoặc mật khẩu không đúng !";
}