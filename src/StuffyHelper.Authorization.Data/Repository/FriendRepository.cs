using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Authorization.Data.Storage;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;

namespace StuffyHelper.Authorization.Data.Repository;

/// <inheritdoc />
public class FriendRepository : IFriendRepository
    {
        private readonly UserDbContext _context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public FriendRepository(UserDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
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
                    throw new EntityAlreadyExistsException("Request for User {UserId} already exists", ex, friendEntry.UserId);

                throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task DeleteFriendAsync(Guid friendId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(friendId, nameof(friendId));

            try
            {
                var friendship = await _context.Friends
                    .FirstOrDefaultAsync(
                    s => s.Id == friendId, cancellationToken);

                if (friendship is null)
                {
                    
                    throw new EntityNotFoundException("Friendship {FriendId} not found.", friendId);
                }


                _context.Friends.Remove(friendship);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<Response<FriendEntry>> GetFriends(string userId, int limit = 20, int offset = 0, CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.Friends
                    .Include(x => x.Friend)
                    .Where(e => e.UserId == userId)
                    .OrderByDescending(e => e.User.FirstName)
                    .ToListAsync(cancellationToken);

                return new Response<FriendEntry>()
                {
                    Data = searchedData.Skip(offset).Take(limit),
                    TotalPages = (int)Math.Ceiling(searchedData.Count() / (double)limit),
                    Total = searchedData.Count()
                };
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }
    }