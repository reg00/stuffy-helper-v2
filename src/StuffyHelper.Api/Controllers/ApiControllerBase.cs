using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace StuffyHelper.Api.Controllers
{
    public class ApiControllerBase : Controller
    {
        private ISender _mediator;
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8601 // Possible null reference assignment.
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8603 // Possible null reference return.
    }
}
