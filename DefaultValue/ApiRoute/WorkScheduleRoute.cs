namespace DefaultValue.ApiRoute;

public record WorkScheduleRoute : ActionRouteBase {
    public const string ControllerName = "WorkSchedules";
    //public static string GetCurrent => $"{ControllerName}/GetCurrent";

    /// <summary>
    /// Lấy danh sách lịch làm việc theo ngày
    /// </summary>
    /// <param name="date">YYYY-MM-dd</param>
    /// <returns></returns>
    public static string GetByDate(DateOnly date) => $"{ControllerName}/GetByDate/{date:yyyy-MM-dd}";
    public static string GetOne(int id) => $"{ControllerName}/GetOne/{id}";
    public static string UpdateList => $"{ControllerName}/UpdateList";
    public static string CreateNextWeek(string userId, DateOnly dateOnly) => $"{ControllerName}/CreateNextWeek/{userId}/{dateOnly:yyyy-MM-dd}";
}