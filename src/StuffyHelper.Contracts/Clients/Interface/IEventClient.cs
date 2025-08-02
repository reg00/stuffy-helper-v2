using Microsoft.AspNetCore.Http;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

/// <summary>
/// Клиент для работы с ивентами
/// </summary>
public interface IEventClient
{
    /// <summary>
    /// Получение списка ивентов
    /// </summary>
    public Task<Response<EventShortEntry>> GetEventsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        string? description = null,
        DateTime? createdDateStart = null,
        DateTime? createdDateEnd = null,
        DateTime? eventDateStartMin = null,
        DateTime? eventDateStartMax = null,
        DateTime? eventDateEndMin = null,
        DateTime? eventDateEndMax = null,
        string? userId = null,
        bool? isCompleted = null,
        bool? isActive = null,
        Guid? participantId = null,
        Guid? purchaseId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение ивента по его идентификатору
    /// </summary>
    public Task<GetEventEntry> GetEventAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление ивента
    /// </summary>
    public Task<EventShortEntry> CreateEventAsync(
        string token,
        AddEventEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление ивента
    /// </summary>
    public Task DeleteEventAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Извенение данных по ивенту
    /// </summary>
    public Task<EventShortEntry> UpdateEventAsync(
        string token,
        Guid eventId,
        UpdateEventEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление обложки ивента
    /// </summary>
    public Task DeleteEventAvatarAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление обложки ивента
    /// </summary>
    public Task<EventShortEntry> UpdateEventAvatarAsync(
        string token,
        Guid eventId,
        IFormFile file,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Завершение ивента
    /// </summary>
    public Task CompleteEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Переоткрытие ивента
    /// </summary>
    public Task ReopenEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Расчет долгов по ивенту
    /// </summary>
    public Task CheckoutEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);
}