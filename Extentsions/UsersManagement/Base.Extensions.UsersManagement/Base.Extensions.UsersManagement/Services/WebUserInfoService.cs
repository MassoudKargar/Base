using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Base.Extensions.UsersManagement.Extensions;
using Base.Extensions.UsersManagement.Options;
using Base.Extensions.UsersManagement.Abstractions;

namespace Base.Extensions.UsersManagement.Services;

public class WebUserInfoService(IHttpContextAccessor httpContextAccessor, IOptions<UserManagementOptions> configuration)
    : IUserInfoService
{
    private readonly UserManagementOptions _configuration = configuration.Value;

    public string GetUserAgent()
    => httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"] ?? _configuration.DefaultUserAgent;

    public string GetUserIp()
    => httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? _configuration.DefaultUserIp;

    public string UserId()
    => httpContextAccessor?.HttpContext?.User?.GetClaim(ClaimTypes.NameIdentifier) ?? string.Empty;

    public string GetUsername()
    => httpContextAccessor.HttpContext?.User?.GetClaim(ClaimTypes.Name) ?? _configuration.DefaultUsername;

    public string GetFirstName()
    => httpContextAccessor.HttpContext?.User?.GetClaim(ClaimTypes.GivenName) ?? _configuration.DefaultFirstName;

    public string GetLastName()
    => httpContextAccessor.HttpContext?.User?.GetClaim(ClaimTypes.Surname) ?? _configuration.DefaultLastName;

    public bool IsCurrentUser(string userId)
    {
        return string.Equals(UserId().ToString(), userId, StringComparison.OrdinalIgnoreCase);
    }

    public string? GetClaim(string claimType)
    => httpContextAccessor.HttpContext?.User?.GetClaim(claimType);

    public string UserIdOrDefault() => UserIdOrDefault(_configuration.DefaultUserId);

    public string UserIdOrDefault(string defaultValue)
    {
        string userId = UserId();
        return string.IsNullOrEmpty(userId) ? defaultValue : userId;
    }
}