namespace Base.Extensions.Messaging;

public class MessagingOptions
{
    public TransportType Type { get; set; }

    public string Server { get; set; } = null!;

    public string PostgresSqlConnection { get; set; } = null!;

    public bool Dashboard { get; set; }

    public string? DashboardUsername { get; set; }

    public string? DashboardPassword { get; set; }

    public bool OpenTelemetryTracing { get; set; }

    public string PathMatch { get; set; } = "/messaging";

    public string? PathBase { get; set; }

    public string? Schema { get; set; } = "messaging";

    public TimeSpan SucceedMessageExpiredAfter { get; set; } = TimeSpan.FromDays(7);

    public TimeSpan FailedMessageExpiredAfter { get; set; } = TimeSpan.FromDays(100);

    public string? UsageMode { get; set; }

    public bool ConcurrentLock { get; set; }

    public ushort? PrefetchCount { get; set; }

    public bool DisableRetriesForReceivedMessages { get; set; }

    public TimeSpan FailedRetryInterval { get; set; } = TimeSpan.FromSeconds(60);

    public TimeSpan FallbackWindowLookBack { get; set; } = TimeSpan.FromMinutes(4);
}