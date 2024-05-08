namespace Base.Utilities.SoftwarePartDetector.Options;

public class SoftwarePartDetectorOptions
{
    public string ApplicationName { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string DestinationServiceBaseAddress { get; set; } = string.Empty;
    public string DestinationServicePath { get; set; } = string.Empty;
    public bool FakeSSL { get; set; } = false;
    public OAuthOption OAuth { get; set; }
}