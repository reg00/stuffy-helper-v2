using System.Security.Claims;

namespace StuffyHelper.Core.Features.Common
{
    public interface IPermissionService
    {
        Task<string?> GetUserId(
            ClaimsPrincipal user,
            string? userId = null,
            CancellationToken cancellationToken = default);
    }
}
