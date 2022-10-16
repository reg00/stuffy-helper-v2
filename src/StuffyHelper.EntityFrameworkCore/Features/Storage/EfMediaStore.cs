using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.EntityFrameworkCore.Features.Schema;
using System.Threading;

namespace StuffyHelper.EntityFrameworkCore.Features.Storage
{
    public class EfMediaStore : IMediaStore
    {
        private readonly StuffyHelperContext _context;

        public EfMediaStore(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<MediaEntry> AddMediaAsync(MediaEntry media, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            try
            {
                media.CreatedDate = DateTime.Now;
                var entry = await _context.Medias.AddAsync(media, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException($"Media with id '{media.Id}' and event name '{media.Event.Name}' already exists", ex);

                else throw new DataStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task DeleteMediaAsync(MediaEntry media, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Medias.Remove(media);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<MediaEntry> GetMediaAsync(Guid mediaId, CancellationToken cancellationToken = default)
        {
            try
            {
                var media = await _context.Medias
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(e=> e.Id == mediaId,
                    cancellationToken);

                if (media is null)
                {
                    throw new ResourceNotFoundException($"Media with id: {mediaId} not found.");
                }

                return media;
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<IEnumerable<MediaEntry>> GetMediasAsync(
            int offset,
            int limit,
            Guid? eventId = null,
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
                    (eventId == null || e.EventId == eventId) &&
                    (createdDateStart == null || e.CreatedDate >= createdDateStart) &&
                    (createdDateEnd == null || e.CreatedDate <= createdDateEnd) && 
                    (mediaType == null || e.MediaType == mediaType))
                    .Skip(offset).Take(limit)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<MediaEntry> GetPrimalEventMedia(Guid eventId, CancellationToken cancellationToken = default)
        {
            try
            {
                var media = await _context.Medias
                    .Include(e => e.Event)
                    .FirstOrDefaultAsync(e => e.EventId == eventId && e.IsPrimal == true,
                    cancellationToken);

                if (media is null)
                {
                    throw new ResourceNotFoundException($"Media with id: {eventId} not found.");
                }

                return media;
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }
    }
}
