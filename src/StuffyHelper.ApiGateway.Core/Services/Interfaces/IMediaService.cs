using StuffyHelper.Common.Client;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    public interface IMediaService
    {
        Task DeleteMediaAsync(string token, Guid mediaId, CancellationToken cancellationToken = default);

        Task<FileParam> GetMediaFormFileAsync(string token, Guid mediaId, CancellationToken cancellationToken = default);

        Task<GetMediaEntry> GetMediaMetadataAsync(string token, Guid mediaId, CancellationToken cancellationToken = default);

        Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
            string token,
            int offset,
            int limit,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default);

        Task<MediaShortEntry> StoreMediaFormFileAsync(
            string token,
            AddMediaEntry media,
            bool isPrimal = false,
            CancellationToken cancellationToken = default);
    }
}
