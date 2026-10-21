namespace Base.EndPoints.Web.Utilities;
public static class Utility
{
    static Utility()
    {
    }

    public static string GetExcelName(string value)
    {
        var result = $"{value}-{DateTimeOffset.Now.Ticks}.xlsx";

        return result;
    }
}