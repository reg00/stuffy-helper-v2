using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Core1.Exceptions;
using StuffyHelper.Authorization.Core1.Features.Friends;
using StuffyHelper.Authorization.Core1.Models;
using StuffyHelper.Authorization.EntityFrameworkCore1.Features.Schema;

namespace StuffyHelper.Authorization.EntityFrameworkCore1.Features.Storage
{
    public class EfFriendStorage : IFriendStore
    {
        private readonly UserDbContext _context;

        public EfFriendStorage(UserDbContext context)
        {
            _context = context;
        }


        public async Task<FriendEntry> AddFriendAsync(FriendEntry friendEntry, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(friendEntry, nameof(friendEntry));

            try
            {
                var entry = await _context.Friends.AddAsync(friendEntry, cancellationToken);
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

        public async Task DeleteFriendAsync(Guid friendshipId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(friendshipId, nameof(friendshipId));

            try
            {
                var friendship = await _context.Friends
                    .FirstOrDefaultAsync(
                    s => s.Id == friendshipId, cancellationToken);

                if (friendship is null)
                {
                    throw new EntityNotFoundException($"Friendship with Id '{friendshipId}' not found.");
                }


                _context.Friends.Remove(friendship);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(ex.Message);
            }
        }

        public async Task<AuthResponse<FriendEntry>> GetFriends(string userId, int limit = 20, int offset = 0, CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.Friends
                    .Include(x => x.Friend)
                    .Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.User.FirstName)
                    .ToListAsync(cancellationToken);

                return new AuthResponse<FriendEntry>()
                {
                    Data = searchedData.Skip(offset).Take(limit),
                    TotalPages = (int)Math.Ceiling(searchedData.Count() / (double)limit),
                    Total = searchedData.Count()
                };
            }
            catch (Exception ex)
            {
                throw new AuthorizationException(ex.Message);
            }
        }
    }
}
