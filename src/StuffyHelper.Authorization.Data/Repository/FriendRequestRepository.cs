using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Authorization.Data.Storage;
using StuffyHelper.Common.Exceptions;

namespace StuffyHelper.Authorization.Data.Repository;

/// <inheritdoc />
public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly UserDbContext _context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public FriendRequestRepository(UserDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<FriendsRequest> GetRequest(Guid requestId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            try
            {
                var entry = await _context.FriendsRequests
                    .Include(x => x.UserTo)
                    .Include(x => x.UserFrom)
                    .FirstOrDefaultAsync(e => e.Id == requestId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException("Friend request {FriendRequestId} not found.", requestId);

                return entry;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<FriendsRequest?> GetRequest(string userId, string friendId, CancellationToken cancellationToken = default)
        {
            var entry = await _context.FriendsRequests
                .Include(x => x.UserTo)
                .Include(x => x.UserFrom)
                .FirstOrDefaultAsync(e => e.UserIdFrom == userId && e.UserIdTo == friendId ||
                    e.UserIdFrom == friendId && e.UserIdTo == userId, cancellationToken);

            return entry;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FriendsRequest>> GetSendedRequestsAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            try
            {
                return await _context.FriendsRequests
                    .Include(x => x.UserTo)
                    .Where(e => e.UserIdFrom == userId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FriendsRequest>> GetIncomingRequestsAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            try
            {
                return await _context.FriendsRequests
                    .Include(x => x.UserFrom)
                    .Where(e => e.UserIdTo == userId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<FriendsRequest> AddRequestAsync(FriendsRequest request, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(request, nameof(request));

            try
            {
                var entry = await _context.FriendsRequests.AddAsync(request, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException("Request for User: {UserId} already exists", ex, request.UserIdFrom);

                throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task DeleteRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            try
            {
                var request = await _context.FriendsRequests
                    .FirstOrDefaultAsync(
                    s => s.Id == requestId, cancellationToken);

                if (request is null)
                {
                    throw new EntityNotFoundException("Friend request {FriendRequestId} not found.", requestId);
                }

                _context.FriendsRequests.Remove(request);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task ComfirmRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            try
            {
                var request = await _context.FriendsRequests
                    .FirstOrDefaultAsync(
                    s => s.Id == requestId, cancellationToken);

                if (request is null)
                {
                    throw new EntityNotFoundException("Friend request {FriendRequestId} not found.", requestId);
                }

                request.IsComfirmed = true;
                _context.FriendsRequests.Update(request);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }
    }