using System.Security.Claims;
using EnsureThat;
using Minio.Exceptions;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Core.Services;

public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IAuthorizationService _authorizationService;

        public FriendService(IFriendRepository friendRepository, IAuthorizationService authorizationService)
        {
            _friendRepository = friendRepository;
            _authorizationService = authorizationService;
        }

        public async Task<FriendShortEntry> AddFriendAsync(
            string userId,
            string friendId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));
            EnsureArg.IsNotNullOrWhiteSpace(friendId, nameof(friendId));

            var stuffyUser = await _authorizationService.GetUserById(userId);
            var friend = await _authorizationService.GetUserById(friendId);

            if (stuffyUser.Id == friend.Id)
                throw new AuthorizationException("Can not request yourself.");

            var request = new FriendEntry(stuffyUser.Id, friend.Id);

            var result = await _friendRepository.AddFriendAsync(request, cancellationToken);
            return new FriendShortEntry(result);
        }

        public async Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(friendshipId, nameof(friendshipId));

            await _friendRepository.DeleteFriendAsync(friendshipId, cancellationToken);
        }

        public async Task<AuthResponse<UserShortEntry>> GetFriends(ClaimsPrincipal user, int limit = 20, int offset = 0, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var stuffyUser = await _authorizationService.GetUserByName(userName);
            var response = await _friendRepository.GetFriends(stuffyUser.Id, limit, offset, cancellationToken);

            return new AuthResponse<UserShortEntry>()
            {
                Data = response.Data.Select(x => new UserShortEntry(x.Friend)),
                Total = response.Total,
                TotalPages = response.TotalPages
            };
        }
    }