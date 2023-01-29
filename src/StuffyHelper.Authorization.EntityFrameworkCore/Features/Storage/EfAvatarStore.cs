using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Authorization.Core.Features.Avatar;
using StuffyHelper.Authorization.EntityFrameworkCore.Features.Schema;

namespace StuffyHelper.Authorization.EntityFrameworkCore.Features.Storage
{
    public class EfAvatarStore : IAvatarStore
    {
        private readonly UserDbContext _context;

        public EfAvatarStore(UserDbContext context)
        {
            _context = context;
        }

        public async Task<AvatarEntry> AddAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(avatar, nameof(avatar));

            try
            {
                avatar.CreatedDate = DateTime.Now;
                var entry = await _context.Avatars.AddAsync(avatar, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new AuthorizationEntityAlreadyExistsException($"User {avatar.UserId} already have avatar", ex);

                else throw new AuthStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new AuthStoreException(ex);
            }
        }

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
                throw new AuthStoreException(ex);
            }
        }

        public async Task DeleteAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Avatars.Remove(avatar);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new AuthStoreException(ex);
            }
        }

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
                    throw new AuthorizationResourceNotFoundException($"Avatar with id: {avatarId} not found.");
                }

                return avatar;
            }
            catch (AuthorizationResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AuthStoreException(ex);
            }
        }

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
                    throw new AuthorizationResourceNotFoundException($"Avatar not found.");
                }

                return avatar;
            }
            catch (AuthorizationResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AuthStoreException(ex);
            }
        }
    }
}
