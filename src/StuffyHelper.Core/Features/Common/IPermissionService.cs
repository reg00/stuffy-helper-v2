using System.Security.Claims;

namespace StuffyHelper.Core.Features.Common
{
    public interface IPermissionService
    {
        /// <summary>
        /// Возвращает идентификатор юзера
        /// </summary>
        /// <param name="user">Данные из токена</param>
        /// <param name="userId">Если null и этот юзер - админ, вернет null, иначе заполнится его Id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<string?> GetUserId(
            ClaimsPrincipal user,
            string? userId = null,
            CancellationToken cancellationToken = default);

        Task<string> GetUserId(
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default);
    }
}
