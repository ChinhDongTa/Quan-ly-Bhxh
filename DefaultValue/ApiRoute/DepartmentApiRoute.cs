namespace DefaultValue.ApiRoute;

public record DepartmentApiRoute : ActionRouteBase {
    public const string ControllerName = "Departments";
    public static string GetOneByEmployeeId(int employeeId)
        => $"{ControllerName}/GetOneByEmployeeId/{employeeId}";
}
