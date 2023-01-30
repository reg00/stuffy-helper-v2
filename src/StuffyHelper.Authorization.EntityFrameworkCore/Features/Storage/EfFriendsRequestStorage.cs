using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Authorization.Core.Features.Friend;
using StuffyHelper.Authorization.EntityFrameworkCore.Features.Schema;

namespace StuffyHelper.Authorization.EntityFrameworkCore.Features.Storage
{
    public class EfFriendsRequestStorage : IFriendsRequestStore
    {
        private readonly UserDbContext _context;

        public EfFriendsRequestStorage(UserDbContext context)
        {
            _context = context;
        }

        public async Task<FriendsRequest> GetRequest(Guid requestId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(requestId, nameof(requestId));

            try
            {
                var entry = await _context.FriendsRequests
                    .FirstOrDefaultAsync(e => e.Id == requestId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException($"Friend request with Id '{requestId}' Not Found.");

                return entry;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(ex.Message);
            }
        }

        public async Task<IEnumerable<FriendsRequest>> GetSendedRequestsAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            try
            {
                return await _context.FriendsRequests
                    .Where(e => e.UserIdFrom == userId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(ex.Message);
            }
        }

        public async Task<IEnumerable<FriendsRequest>> GetIncomingRequestsAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            try
            {
                return await _context.FriendsRequests
                    .Where(e => e.UserIdTo == userId)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(ex.Message);
            }
        }

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
                    throw new EntityAlreadyExistsException($"Request already exists", ex);

                else throw new AuthorizationException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(ex.Message);
            }
        }

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
                    throw new EntityNotFoundException($"Request with Id '{requestId}' not found.");
                }

                _context.FriendsRequests.Remove(request);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(ex.Message);
            }
        }

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
                    throw new EntityNotFoundException($"Request with Id '{requestId}' not found.");
                }

                request.IsComfirmed = true;
                _context.FriendsRequests.Update(request);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(ex.Message);
            }
        }
    }
}
