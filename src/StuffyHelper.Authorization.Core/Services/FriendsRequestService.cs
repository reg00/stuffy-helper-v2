using System.Security.Claims;
using AutoMapper;
using EnsureThat;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Common.Exceptions;

namespace StuffyHelper.Authorization.Core.Services;

/// <inheritdoc />
 public class FriendsRequestService : IFriendsRequestService
    {
        private readonly IFriendRequestRepository _requestRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IFriendService _friendService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor.
        /// </summary>
        public FriendsRequestService(
            IFriendRequestRepository requestRepository,
            IAuthorizationService authorizationService,
            IFriendService friendService,
            IMapper mapper)
        {
            _requestRepository = requestRepository;
            _authorizationService = authorizationService;
            _friendService = friendService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task ConfirmRequest(Guid requestId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            var request = await _requestRepository.GetRequest(requestId, cancellationToken);
            
            await _friendService.AddFriendAsync(request.UserIdFrom, request.UserIdTo, cancellationToken);
            await _friendService.AddFriendAsync(request.UserIdTo, request.UserIdFrom, cancellationToken);
            await _requestRepository.ComfirmRequestAsync(requestId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<FriendsRequestShort> GetRequestAsync(Guid requestId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            var entry = await _requestRepository.GetRequest(requestId, cancellationToken);
            return _mapper.Map<FriendsRequestShort>(entry);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FriendsRequestShort>> GetSendedRequestsAsync(
           ClaimsPrincipal user,
           CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var stuffyUser = await _authorizationService.GetUserByName(userName);
            var resp = await _requestRepository.GetSendedRequestsAsync(stuffyUser.Id, cancellationToken);

            return resp.Select(x => _mapper.Map<FriendsRequestShort>(x));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FriendsRequestShort>> GetIncomingRequestsAsync(
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));

            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var stuffyUser = await _authorizationService.GetUserByName(userName);
            var resp = await _requestRepository.GetIncomingRequestsAsync(stuffyUser.Id, cancellationToken);

            return resp.Select(x => _mapper.Map<FriendsRequestShort>(x));
        }

        /// <inheritdoc />
        public async Task<FriendsRequestShort> AddRequestAsync(
            ClaimsPrincipal friend,
            string userId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));
            EnsureArg.IsNotNull(friend, nameof(friend));

            var userName = friend.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var incomingUser = await _authorizationService.GetUserByName(userName);
            var requestUser = await _authorizationService.GetUserById(userId);

            if (incomingUser.Id == requestUser.Id)
                throw new AuthorizationException("Can not request yourself.");

            var request = _mapper.Map<FriendsRequest>((incomingUser.Id, requestUser.Id));
            var result = await _requestRepository.AddRequestAsync(request, cancellationToken);
            
            return _mapper.Map<FriendsRequestShort>(result);
        }

        /// <inheritdoc />
        public async Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            await _requestRepository.DeleteRequestAsync(requestId, cancellationToken);
        }
    }