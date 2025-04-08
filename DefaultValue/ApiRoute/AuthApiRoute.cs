namespace DefaultValue.ApiRoute;

public record AccountApiRoute {
    public const string ControllerName = "Account";
    //public static string Login => $"{ControllerName}/Login";
    //public static string RefreshToken => $"{ControllerName}/RefreshToken";
    public static string Register => $"{ControllerName}/Register";
    public static string Info => $"{ControllerName}/Info";
    //public static string ChangePassword => $"{ControllerName}/ChangePassword";
}