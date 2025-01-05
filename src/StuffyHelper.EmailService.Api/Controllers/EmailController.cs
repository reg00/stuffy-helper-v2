using System.Net;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Common.Web;
using StuffyHelper.EmailService.Contracts.Models;
using StuffyHelper.EmailService.Core.Service.Interfaces;

namespace StuffyHelper.EmailService.Api.Controllers;

public class EmailController : Controller
{
    private readonly IStuffyEmailService _emailService;
    private readonly ILogger<EmailController> _logger;

    public EmailController(IStuffyEmailService emailService, ILogger<EmailController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [Route(KnownRoutes.SendEmailRoute)]
    public async Task SendAsync([FromBody] SendEmailRequest request)
    {
        _logger.LogInformation("Start to send email");
        await _emailService.SendEmailAsync(request);
    }
}