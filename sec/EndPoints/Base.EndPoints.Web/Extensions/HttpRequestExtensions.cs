namespace Base.EndPoints.Web.Extensions;

public static class HttpRequestExtensions
{

    public static string GetIP(this HttpContext httpContext, bool tryUseXForwardHeader = true)
    {
        string text = string.Empty;
        if (tryUseXForwardHeader)
        {
            text = SplitCsv(httpContext.GetHeaderValue("X-Forwarded-For")).FirstOrDefault();
        }
        if (string.IsNullOrWhiteSpace(text) && httpContext?.Connection?.RemoteIpAddress != null)
        {
            text = httpContext.Connection.RemoteIpAddress.ToString();
        }
        if (string.IsNullOrWhiteSpace(text))
        {
            text = httpContext.GetHeaderValue("REMOTE_ADDR");
        }
        return text;
    }

    public static string GetHeaderValue(this HttpContext httpContext, string headerName)
    {
        StringValues value = "";
        return (httpContext?.Request?.Headers?
            .TryGetValue(headerName, out value))
            .GetValueOrDefault() ? value.ToString() : string.Empty;
    }

    private static List<string> SplitCsv(string csvList, bool nullOrWhitespaceInputReturnsNull = false)
    {
        if (string.IsNullOrWhiteSpace(csvList))
        {
            return !nullOrWhitespaceInputReturnsNull ? new List<string>() : null;
        }
        return (from s in csvList.TrimEnd(',').Split(',').AsEnumerable()
                select s.Trim()).ToList();
    }
}