
using System.Net.NetworkInformation;
using Base.Utility.Cryptography;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Base.EndPoints.Web.Middlewares;
internal class ApplicationInformationMiddleware
{
    private static DateTime _startApplicationTime;

    public static string BuildInfo(HttpContext context)
    {

        var buildDate = File.GetLastWriteTime(Assembly.GetEntryAssembly().Location);
        var info = new
        {
            Name = Assembly.GetEntryAssembly().GetName().Name,
            Version = Assembly.GetEntryAssembly().GetName().Version.ToString(),
            ErfanFrameworkVersion = Assembly.GetExecutingAssembly().GetName().Version,
            FrameworkVersion = Environment.Version,
            FrameworkRuntimeVersion = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            BuildDate = buildDate,
            RelativeBuildDate = Relative(buildDate),
            ApplicationeStartDate = _startApplicationTime,
            ApplicationStartDate = _startApplicationTime,
            RelativeApplicationeStartDate = Relative(_startApplicationTime),
            RelativeApplicationStartDate = Relative(_startApplicationTime),
            //Drives = string.Join(",", Environment.GetLogicalDrives()),
            RemoteIpAddress = context.Connection.RemoteIpAddress?.ToString(),
            Environment.MachineName,
            MachineUptime = Relative(DateTime.Now.AddMilliseconds(Environment.TickCount64 * -1)).Replace(" ago", ""),
            MachineTime = DateTimeOffset.Now,
            MachineTimeZone = TimeZoneInfo.Local.StandardName.ToString(),
            MachineOS = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
            PhysicalAddresses = GetPhysicalAddresses(),
            WorkerID = WorkerIdGenerator.GenerateWorkerId()
        };

        return JsonSerializer.Serialize(info);
    }

    internal static void StartApplicationTime()
    {
        _startApplicationTime = DateTime.Now;
    }

    private static string Relative(DateTimeOffset dateTimeOffset)
    {
        return Relative(dateTimeOffset.ToUniversalTime());
    }

    private static string Relative(DateTime dateTime)
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        var ts = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
        double delta = Math.Abs(ts.TotalSeconds);

        if (delta < 1 * MINUTE)
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

        if (delta < 2 * MINUTE)
            return "a minute ago";

        if (delta < 45 * MINUTE)
            return ts.Minutes + " minutes ago";

        if (delta < 90 * MINUTE)
            return "an hour ago";

        if (delta < 24 * HOUR)
            return ts.Hours + " hours ago";

        if (delta < 48 * HOUR)
            return "yesterday";

        if (delta < 30 * DAY)
            return ts.Days + " days ago";

        if (delta < 12 * MONTH)
        {
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }
        else
        {
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }

    private static string GetPhysicalAddresses()
    {
        try
        {
            var addresess = NetworkInterface.GetAllNetworkInterfaces()
                .Select(c => $"{c.Name}[{c.NetworkInterfaceType:g}|{c.GetPhysicalAddress()}]");

            return string.Join(",", addresess);
        }
        catch (Exception ex)
        {
            return $"failed get physical addresses {ex.Message}";
        }
    }
}