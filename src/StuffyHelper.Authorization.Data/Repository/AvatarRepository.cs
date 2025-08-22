using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Authorization.Data.Storage;
using StuffyHelper.Common.Exceptions;

namespace StuffyHelper.Authorization.Data.Repository;

/// <inheritdoc />
public class AvatarRepository : IAvatarRepository
    {
        private readonly UserDbContext _context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public AvatarRepository(UserDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<AvatarEntry> AddAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(avatar, nameof(avatar));

            try
            {
                avatar.CreatedDate = DateTime.UtcNow;
                var entry = await _context.Avatars.AddAsync(avatar, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException("User: {UserId} already have avatar", ex, avatar.UserId);

                throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<AvatarEntry> UpdateAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(avatar, nameof(avatar));

            try
            {
                var entry = _context.Avatars.Update(avatar);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task DeleteAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Avatars.Remove(avatar);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<AvatarEntry> GetAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default)
        {
            try
            {
                var avatar = await _context.Avatars
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e => e.Id == avatarId,
                    cancellationToken);

                if (avatar is null)
                {
                    throw new EntityNotFoundException("Avatar: {AvatarId} not found.", avatarId);
                }

                return avatar;
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
        public async Task<AvatarEntry> GetAvatarAsync(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var avatar = await _context.Avatars
                    .Include(e => e.User)
                    .FirstOrDefaultAsync(e => e.UserId == userId,
                    cancellationToken);

                if (avatar is null)
                {
                    throw new EntityNotFoundException("Avatar for User: {UserId} not found.", userId);
                }

                return avatar;
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
    }