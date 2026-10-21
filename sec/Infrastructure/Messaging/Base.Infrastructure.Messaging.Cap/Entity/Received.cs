namespace Base.Infrastructure.Messaging.Cap.Entity;

public class Received
{
    public long Id { get; set; }

    public string Version { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Group { get; set; }

    public string? Content { get; set; }

    public int Retries { get; set; }

    public DateTime Added { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string StatusName { get; set; } = null!;
}