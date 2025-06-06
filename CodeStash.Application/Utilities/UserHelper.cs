using CodeStash.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CodeStash.Application.Utilities;
public class UserHelper(IHttpContextAccessor httpContextAccessor)
{
    public string GetUserId()
    {
        return httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new InvalidOperationException("User ID not found in claims.");
    }

    public string GetUserEmail()
    {
        return httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Email)?.Value
            ?? throw new InvalidOperationException("User email not found in claims.");
    }

    public string GetUserRole()
    {
        return httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Role)?.Value
            ?? throw new InvalidOperationException("User role not found in claims.");
    }

    public Roles GetUserRoleAsEnum()
    {
        var roleString = GetUserRole();
        if (Enum.TryParse<Roles>(roleString, out var roleEnum))
        {
            return roleEnum;
        }
        throw new InvalidOperationException($"User role '{roleString}' is not a valid Roles enum value.");
    }

    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext?.User
            .Identity?.IsAuthenticated ?? false;
    }
}
