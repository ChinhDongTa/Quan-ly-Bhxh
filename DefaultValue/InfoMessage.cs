namespace DefaultValue;
public record InfoMessage {
    public static string PostFailed => Resources.PostFailed;
    public static string DownloadFailed => Resources.DownloadFailed;
    public static string Unknow => Resources.Unknown;
    public static string DownloadSuccess => Resources.DownloadSuccess;
    public static string Success => Resources.Success;
    public static string Unselect(string name) => string.Format(Resources.Unselect, name);
    public static string ObjectNull(string objName) => string.Format(Resources.ObjectNull, objName);
    public static string NotAuthorized => Resources.NotAuthorized;
    public static string NotFound(string objectName) => string.Format(Resources.NotFound, objectName);
    public static string InputEmpty => Resources.InputEmpty;
    public static string InvalidId(string name) => string.Format(Resources.InvalidId, name);
    public static string ActionSuccess(CRUD actionName, string objName) => string.Format(Resources.ActionSuccess, actionName.CRUD2String(), objName);
    public static string ActionFailed(CRUD actionName, string objName) => string.Format(Resources.ActionFailed, actionName.CRUD2String(), objName);
    public static string CrudResult(CRUD actionName, bool? result, string objName) => (result ?? false) ? ActionSuccess(actionName, objName) : ActionFailed(actionName, objName);
    //public static string ActionSuccess(string actionName) => string.Format(Resources.ActionSuccess, actionName);
    //public static string ActionFailed(string actionName) => string.Format(Resources.ActionFailed, actionName);
    public static string EmailOrPasswordInvalid => Resources.EmailOrPasswordInvalid;
}