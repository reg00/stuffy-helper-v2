using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Client;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services
{
    public class MediaService : IMediaService
    {
        private readonly IMediaClient _mediaClient;
        
        public MediaService(IMediaClient mediaClient)
        {
            _mediaClient = mediaClient;
        }

        public async Task DeleteMediaAsync(string token, Guid mediaId, CancellationToken cancellationToken = default)
        {
            await _mediaClient.DeleteMediaAsync(token, mediaId, cancellationToken);
        }

        public async Task<FileParam> GetMediaFormFileAsync(string token, Guid mediaId, CancellationToken cancellationToken = default)
        {
            return await _mediaClient.RetrieveMediaFormFileAsync(token, mediaId, cancellationToken);
        }

        public async Task<GetMediaEntry> GetMediaMetadataAsync(string token, Guid mediaId, CancellationToken cancellationToken = default)
        {
            return await _mediaClient.GetMediaMetadataAsync(token, mediaId, cancellationToken);
        }

        public async Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
            string token,
            int offset,
            int limit,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default)
        {
            return await _mediaClient.GetMediaMetadatasAsync(token,
                offset, limit, eventId, createdDateStart, createdDateEnd, mediaType, cancellationToken);
        }

        public async Task<MediaShortEntry> StoreMediaFormFileAsync(
            string token,
            AddMediaEntry media,
            bool isPrimal = false,
            CancellationToken cancellationToken = default)
        {
            return await _mediaClient.StoreMediaFormFileAsync(token, media, cancellationToken);
        }
    }
}
