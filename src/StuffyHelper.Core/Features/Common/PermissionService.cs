using StuffyHelper.Authorization.Core1.Features;
using StuffyHelper.Authorization.Core1.Models;
using System.Security.Claims;

namespace StuffyHelper.Core.Features.Common
{
    public class PermissionService : IPermissionService
    {
        private readonly IAuthorizationService _authorizationService;

        public PermissionService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<string?> GetUserId(
            ClaimsPrincipal user,
            string? userId = null,
            CancellationToken cancellationToken = default)
        {
            if (user.IsInRole(nameof(UserType.Admin)))
            {
                return userId;
            }
            else
            {
                var currentUser = await _authorizationService.GetUserByToken(user, cancellationToken);
                return currentUser.Id;
            }
        }

        public async Task<string> GetUserId(
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default)
        {
            var currentUser = await _authorizationService.GetUserByToken(user, cancellationToken);
            return currentUser.Id;
        }
    }
}
