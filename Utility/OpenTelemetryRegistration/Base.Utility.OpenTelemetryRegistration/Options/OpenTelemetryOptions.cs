namespace Base.Utilities.OpenTelemetryRegistration.Options;
public class OpenTelemetryOptions
{
    public required string ApplicationName { get; set; }
    public required string ServiceName { get; set; }
    public required string ServiceVersion { get; set; }
    public required string ServiceId { get; set; }
    public ExportProcessorType ExportProcessorType { get; set; } = ExportProcessorType.Simple;
    public string OltpEndpoint { get; set; } = "http://localhost:4317";
    public double SamplingProbability { get; set; } = 0.25;
}
