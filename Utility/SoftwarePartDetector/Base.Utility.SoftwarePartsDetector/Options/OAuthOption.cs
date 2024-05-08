namespace Base.Utilities.SoftwarePartDetector.Options;

public class OAuthOption
{
    public bool Enabled { get; set; } = true;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
    public string[] Scopes { get; set; } = Array.Empty<string>();
}