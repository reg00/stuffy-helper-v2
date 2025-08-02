using StuffyHelper.Common.Client;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IMediaClient
{
    /// <summary>
    /// Добавление файла к ивенту
    /// </summary>
    public Task<MediaShortEntry> StoreMediaFormFileAsync(
        string token,
        AddMediaEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение файла
    /// </summary>
    public Task<FileParam> RetrieveMediaFormFileAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение метаданных файла
    /// </summary>
    public Task<GetMediaEntry> GetMediaMetadataAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление файла
    /// </summary>
    public Task DeleteMediaAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение списка метаданных файлов
    /// </summary>
    public Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        DateTimeOffset? createdDateStart = null,
        DateTimeOffset? createdDateEnd = null,
        MediaType? mediaType = null,
        CancellationToken cancellationToken = default);
}