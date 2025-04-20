namespace Base.Utilities.OpenTelemetryRegistration.Monitoring;
public class MetricReporter
{
    private readonly Counter<int> _requestCounter;
    private readonly Histogram<double> _responseTimeHistogram;

    public string MetricName { get; }

    public MetricReporter(string meterName, string prefix)
    {
        if (string.IsNullOrEmpty(meterName))
            throw new ArgumentException("Meter name cannot be null or empty", nameof(meterName));
        if (string.IsNullOrEmpty(prefix))
            throw new ArgumentException("Prefix cannot be null or empty", nameof(prefix));

        var meter = new Meter(meterName);
        MetricName = meterName;

        _requestCounter = meter.CreateCounter<int>(
            name: $"{prefix}_total_request",
            unit: "hits",
            description: "the total number of requests");

        _responseTimeHistogram = meter.CreateHistogram<double>(
            name: $"{prefix}_request_duration_seconds",
            unit: "double",
            description: "The duration in seconds between the response to a request.");
    }

    public void RegisterRequest(string? path)
        => _requestCounter.Add(1, new KeyValuePair<string, object?>("path", path));

    public void RegisterResponseTime(int statusCode, string httpMethod, string? path, TimeSpan elapsed)
    {
        var keyValuePairs = new KeyValuePair<string, object?>[3]
        {
            new("statusCode", statusCode),
            new("httpMethod", httpMethod),
            new("path", path)
        };
        var tags = new TagList(keyValuePairs);
        _responseTimeHistogram.Record(elapsed.TotalSeconds, tags);
    }

}
