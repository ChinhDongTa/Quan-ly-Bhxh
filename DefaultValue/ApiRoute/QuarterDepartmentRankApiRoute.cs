namespace DefaultValue.ApiRoute;

public record QuarterDepartmentRankApiRoute : ActionRouteBase {
    public const string ControllerName = "QuarterDepartmentRanks";
    public static string GetByQuarter(byte q, int year) =>
        $"{ControllerName}/GetByQuarter/{q}/{year}";
    public static string GetCurrentByUserId(string userId)
        => $"{ControllerName}/GetCurrentByUserId/{userId}";

    public static string GetCurrentByDeptId(int deptId)
        => $"{ControllerName}/GetCurrentByDeptId/{deptId}";
}
