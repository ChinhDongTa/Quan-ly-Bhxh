namespace DefaultValue.ApiRoute;

public record AccountApiRoute {
    public const string ControllerName = "Account";
    public static string GetRoles => $"{ControllerName}/GetRoles";
    public static string GetAllRoleName => $"{ControllerName}/GetAllRoleName";
    public static string Register => $"{ControllerName}/Register";
    public static string Info => $"{ControllerName}/Info";
    public static string All => $"{ControllerName}/All";
    public static string AddRoleToUser => $"{ControllerName}/AddRoleToUser";
    public static string RemoveRoleFromUser => $"{ControllerName}/RemoveRoleFromUser";
    public static string Delete(string Id) => $"{ControllerName}/Delete/{Id}";
    public static string GetOne(string Id) => $"{ControllerName}/GetOne/{Id}";
}