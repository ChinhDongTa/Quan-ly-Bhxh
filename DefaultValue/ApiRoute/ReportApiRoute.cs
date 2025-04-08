namespace DefaultValue.ApiRoute;

public record ReportApiRoute {
    public const string ControllerName = "Reports";
    public static string BienBanHopQuy => $"{ControllerName}/BienBanHopQuy";
    public static string TongHopQuyPL4 => $"{ControllerName}/TongHopQuyPL4";
}
