namespace DefaultValue.ApiRoute;

public record ActionRouteBase {
    public static string Create(string nameOfController) => $"{nameOfController}/{ActionBase.Create}";
    public static string Delete(string nameOfController, int id) => $"{nameOfController}/{ActionBase.Delete}/{id}";
    public static string GetOne(string nameOfController, int id) => $"{nameOfController}/{ActionBase.GetOne}/{id}";
    public static string All(string nameOfController) => $"{nameOfController}/{ActionBase.All}";
    public static string Update(string nameOfController, int id) => $"{nameOfController}/{ActionBase.Update}/{id}";
}
