using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Common.Contracts;

namespace StuffyHelper.Core.Helpers;

/// <summary>
/// Helper for work with permissions
/// </summary>
public static class PermissionHelper
{
    public static string? GetUserId(StuffyClaims claims, string? userId = null)
    {
        return claims.Roles.Contains(nameof(UserType.Admin)) ? userId : claims.UserId;
    }
}