using System.Security.Claims;
using AutoMapper;
using EnsureThat;
using Minio.Exceptions;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Core.Services;

/// <inheritdoc />
public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;
        
        /// <summary>
        /// Ctor.
        /// </summary>
        public FriendService(IFriendRepository friendRepository, IAuthorizationService authorizationService, IMapper mapper)
        {
            _friendRepository = friendRepository;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }

        /// <inheritdoc />
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

            var request = _mapper.Map<FriendEntry>((stuffyUser.Id, friend.Id));

            var result = await _friendRepository.AddFriendAsync(request, cancellationToken);
            
            return _mapper.Map<FriendShortEntry>(result);
        }

        /// <inheritdoc />
        public async Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(friendshipId, nameof(friendshipId));

            await _friendRepository.DeleteFriendAsync(friendshipId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<Response<UserShortEntry>> GetFriends(ClaimsPrincipal user, int limit = 20, int offset = 0, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var stuffyUser = await _authorizationService.GetUserByName(userName);
            var response = await _friendRepository.GetFriends(stuffyUser.Id, limit, offset, cancellationToken);

            return new Response<UserShortEntry>()
            {
                Data = response.Data.Select(x => _mapper.Map<UserShortEntry>(x)),
                Total = response.Total,
                TotalPages = response.TotalPages
            };
        }
    }