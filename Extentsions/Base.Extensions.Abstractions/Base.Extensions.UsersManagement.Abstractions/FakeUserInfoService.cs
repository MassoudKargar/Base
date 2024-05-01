namespace Base.Extensions.UsersManagement.Abstractions;

public class FakeUserInfoService(string defaultUserId) : IUserInfoService
{
    private readonly string _defaultUserId = defaultUserId;

    public FakeUserInfoService() : this("1")
    {

    }

    public string? GetClaim(string claimType)
    {
        return claimType;
    }

    public string GetFirstName()
    {
        return "FirstName";
    }

    public string GetLastName()
    {
        return "LastName";
    }

    public string GetUserAgent()
    {
        return "1";
    }

    public string GetUserIp()
    {
        return "0.0.0.0";
    }

    public string GetUsername()
    {
        return "Username";
    }

    public bool HasAccess(string access)
    {
        return true;
    }

    public bool IsCurrentUser(string userId)
    {
        return true;
    }

    public string UserId()
    {
        return "1";
    }

    public string UserIdOrDefault() => _defaultUserId;

    public string UserIdOrDefault(string defaultValue) => defaultValue;
}
