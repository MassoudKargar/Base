namespace Base.Core.Domains.Contracts.Events;

public class DomainEventUserInfo
{
    public long? UserId { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public long? ClientId { get; set; }
    public string? Panel { get; set; }
    public string? OsPlatform { get; set; }

    public DomainEventUserInfo(long? userId, string? username, string? fullName, string? email, string? mobile, long? clientId, string? panel, string? osPlatform)
    {
        UserId = userId;
        Username = username;
        FullName = fullName;
        Email = email;
        Mobile = mobile;
        ClientId = clientId;
        Panel = panel;
        OsPlatform = osPlatform;
    }
}