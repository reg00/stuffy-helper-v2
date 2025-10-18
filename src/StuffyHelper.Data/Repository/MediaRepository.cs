using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Data.Storage;

namespace StuffyHelper.Data.Repository
{
    /// <inheritdoc />
    public class MediaRepository : IMediaRepository
    {
        private readonly StuffyHelperContext _context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public MediaRepository(StuffyHelperContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<MediaEntry> AddMediaAsync(MediaEntry media, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            try
            {
                media.CreatedDate = DateTime.UtcNow;
                var entry = await _context.Medias.AddAsync(media, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException("Media: {MediaId} for Event: {EventId} already exists", ex, media.Id, media.EventId);

                else throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task DeleteMediaAsync(MediaEntry media, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Medias.Remove(media);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<MediaEntry> GetMediaAsync(Guid eventId, Guid mediaId, CancellationToken cancellationToken = default)
        {
            try
            {
                var media = await _context.Medias
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(e => e.Id == mediaId && e.EventId == eventId,
                    cancellationToken);

                if (media is null)
                {
                    throw new EntityNotFoundException("Media {MediaId} not found.", mediaId);                }

                return media;
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
        public async Task<IEnumerable<MediaEntry>> GetMediasAsync(
            Guid eventId,
            int offset,
            int limit,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Medias
                    .Include(e => e.Event)
                    .Where(e =>
                    e.EventId == eventId &&
                    (createdDateStart == null || e.CreatedDate >= createdDateStart) &&
                    (createdDateEnd == null || e.CreatedDate <= createdDateEnd) &&
                    (mediaType == null || e.MediaType == mediaType))
                    .Skip(offset).Take(limit)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<MediaEntry?> GetPrimalEventMedia(Guid eventId, CancellationToken cancellationToken = default)
        {
            return await _context.Medias
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.EventId == eventId && e.IsPrimal == true,
                    cancellationToken);
        }
    }
}
