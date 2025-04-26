namespace DefaultValue.ApiRoute;

public record DepartmentApiRoute : ActionRouteBase {
    public const string ControllerName = "Departments";
    public static string GetOneByEmployeeId(int employeeId)
        => $"{ControllerName}/GetOneByEmployeeId/{employeeId}";
}

public record EventLogApiRoute {
    public const string ControllerName = "EventLogs";
    public static string GetByEmployeeId(int employeeId)
        => $"{ControllerName}/GetByEmployeeId/{employeeId}";
    public static string GetByUserId(string userId)
        => $"{ControllerName}/GetByUserId/{userId}";
    public static string GetByUserName(string email)
        => $"{ControllerName}/GetByUserName/{email}";
    public static string GetTop(int top = 15) =>
        $"{ControllerName}/GetTop/{top}";
}