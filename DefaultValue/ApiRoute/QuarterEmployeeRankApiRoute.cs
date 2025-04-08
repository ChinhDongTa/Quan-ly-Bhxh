﻿namespace DefaultValue.ApiRoute;

public record QuarterEmployeeRankApiRoute : ActionRouteBase {
    public const string ControllerName = "QuarterEmployeeRanks";

    public static string GetByQuarter(byte quarter, int year) =>
        $"{ControllerName}/GetByQuarter/{quarter}/{year}";
    public static string GetCurrentByUserId(string userId) =>
       $"{ControllerName}/GetCurrentByUserId/{userId}";

    public static string GetCurrentByDeptId(int deptId) =>
      $"{ControllerName}/GetCurrentByDeptId/{deptId}";
    public static string Get3QuarterBeforeByDeptId(int deptId) =>
      $"{ControllerName}/Get3QuarterBeforeByDeptId/{deptId}";

    public static string Get3QuarterBeforeByEmployeeId(int employeeId) =>
     $"{ControllerName}/Get3QuarterBeforeByEmployeeId/{employeeId}";
}
