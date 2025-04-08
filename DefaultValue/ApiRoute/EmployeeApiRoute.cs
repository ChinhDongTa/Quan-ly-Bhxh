namespace DefaultValue.ApiRoute;

public record EmployeeApiRoute : ActionRouteBase {
    public const string ControllerName = "Employees";
    public static string FindByName(string name, bool quitJob = false)
        => $"{ControllerName}/FindByName/{name}/{quitJob}";
    public static string GetOneByEmployeeId(int employeeId)
        => $"{ControllerName}/GetOneByEmployeeId/{employeeId}";
    public static string GetByDeptId(int departmentId)
        => $"{ControllerName}/GetByDeptId/{departmentId}";

    public static string GetByUserId(string userId) => $"{ControllerName}/GetByUserId/{userId}";
    public static string GetDeptHeadByUserId(string userId) => $"{ControllerName}/GetDeptHeadByUserId/{userId}";
    public static string GetEmployeeDto => $"{ControllerName}/GetEmployeeDto";
    public static string GetEmployeeDtoForListBox(string name = "All") => $"{ControllerName}/GetEmployeeDtoForListBox/{name}";
}