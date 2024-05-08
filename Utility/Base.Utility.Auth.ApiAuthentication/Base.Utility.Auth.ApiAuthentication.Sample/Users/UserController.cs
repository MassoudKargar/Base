using Microsoft.AspNetCore.Mvc;
using Base.Utilities.Auth.ApiAuthentication.Sample.Users.Models;

namespace Base.Utilities.Auth.ApiAuthentication.Sample.Users;

[ApiController]
[Route("[controller]")]
public class UserController(IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    [HttpGet("[action]")]
    public IEnumerable<UserClaim> GetClaims()
        => _httpContextAccessor?.HttpContext?.User.Claims
        .Select(claim => new UserClaim
        {
            Type = claim.Type,
            Value = claim.Value
        }).ToList() ?? [];
}