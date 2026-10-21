namespace Base.Infrastructure.Messaging.Cap.Entity;
public partial class Published
{
    public long Id { get; set; }

    public string Version { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Content { get; set; }

    public int Retries { get; set; }

    public DateTime Added { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string StatusName { get; set; } = null!;
}